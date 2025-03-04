<template>
  <div>
    <van-row v-if="message !== null && message !== ''" class="error-message">
      {{ message }}
    </van-row>
    <van-list :loading="loading" :finished="finished" finished-text="なし" class="order-list" @load="onLoad">
      <div v-for="(item, key) in map" :key="key">
        <van-row class="time-title">
          {{ key }}
        </van-row>
        <van-row v-for="(obj, index) in item" :key="index" class="order-con">
          <van-col span="21" class="left">
            <van-row class="title">{{ obj.orderProductNameCh }}</van-row>
            <van-row class="txt-light">
              {{ obj.orderProductNameJp }}
            </van-row>

            <!-- 有套餐显示套餐，打包显示打包 -->
            <van-row class="txt-light">
              <span v-if="obj.orderProductSetName != null && obj.orderProductSetName != ''">
                {{ obj.orderProductSetName }}
              </span>
              <span v-if="obj.orderTakeoutFlg" class="price">持ち帰り</span>
            </van-row>
          </van-col>
          <van-col span="3" class="total-price">
            <!-- 折扣时价格显示红色，加上price-red即可；反之，去掉即可 -->
            <van-col class="title price">￥{{ formatAmount(obj.orderDiscountPrice) }}</van-col>
            <van-col class="txt-light num">
              {{ obj.orderQuantity }}個
              <span @click="outputReport(obj)" class="pop-txt" style="margin-top:2px;background-color: #BF9000;">
                伝
              </span>
            </van-col>
            <van-col
              style="padding: 0 5px;"
              v-if="obj.orderRemarks !== null && obj.orderRemarks !== ''"
              @click="showDialog(obj)"
            >
              <span class="pop-txt" style="margin-top:2px;">...</span>
            </van-col>
          </van-col>
        </van-row>
      </div>
    </van-list>

    <van-row class="order-total">
      <span>計</span>
      <span>{{ total }}</span>
      <span class="txt">個</span>
      <span class="txt">￥</span>
      <span>{{ formatAmount(totalPrice) }}</span>
    </van-row>

    <!-- 圆弹出框 -->
    <van-popup v-model="showRemarks" class="order-remarks">
      <van-row class="title">
        {{ orderRemarksItem.nameCh }}
      </van-row>

      <van-field v-model="remarks" rows="2" autosizetype="textarea" readonly />

      <van-row class="row-btn">
        <van-button type="default" class="cancel fl" @click="showRemarks = false">OK</van-button>
      </van-row>
    </van-popup>
  </div>
</template>
<script>
import { selectOrderHistoryData, cookingInstFlgUpdate, cookingInstSlipPrt } from '@/api/cma/cma120'
export default {
  name: 'OrderHistory',
  data() {
    return {
      message: null,
      remarks: '',
      showRemarks: false,
      total: 0,
      totalPrice: 0,
      border: true,
      loading: false,
      finished: false,
      seatLevel: '',
      seatNumber: '',
      map: {},
      receptionBranchNumber: '',
      receptionNumber: '',
      // 下拉
      value1: 0,
      option1: [],
      orderRemarksItem: {},
      queryParam: {}
    }
  },
  created() {
    if (
      sessionStorage.getItem('seatInfo') &&
      sessionStorage.getItem('seatInfo') !== null &&
      sessionStorage.getItem('seatInfo') !== undefined
    ) {
      const queryParam = JSON.parse(sessionStorage.getItem('seatInfo'))
      this.receptionBranchNumber = queryParam.mainSeat.receptionBranchNumber
      this.receptionNumber = queryParam.mainSeat.receptionNumber
      this.seatLevel = queryParam.mainSeat.seatLevel
      this.seatNumber = queryParam.mainSeat.seatNumber
      this.queryParam = queryParam
    }
    this.getList()
  },
  methods: {
    getList() {
      this.message = null
      selectOrderHistoryData({
        receptionBranchNumber: this.receptionBranchNumber,
        receptionNumber: this.receptionNumber
      }).then(response => {
        if (response.status === 200) {
          const orderHistoryList = response.orderHistoryList
          // 戻り値が”正常”（0件）の場合
          if (orderHistoryList.length === 0) {
            this.$msgUtil.messageNew('E_00060', '', this)
          }
          for (const i in orderHistoryList) {
            if (this.map[orderHistoryList[i].orderDatetime]) {
              this.map[orderHistoryList[i].orderDatetime].push(orderHistoryList[i])
            } else {
              const array = []
              array.push(orderHistoryList[i])
              this.map[orderHistoryList[i].orderDatetime] = array
            }
          }
          this.total = response.orderQuantityTotal
          this.totalPrice = response.orderPriceTotal
        } else if (response.status === 601) {
          this.$msgUtil.messageNew('E_00010', '注文履歴', this)
        } else if (response.status === 602) {
          this.$msgUtil.messageNew('E_00010', '注文履歴', this)
        }
      })
    },
    showDialog(item) {
      this.showRemarks = true
      this.remarks = item.orderRemarks
      this.orderRemarksItem = item
    },
    outputReport(item) {
      const param = {
        seatLevel: this.seatLevel,
        seatNumber: this.seatNumber,
        receptionBranchNumber: this.receptionBranchNumber,
        receptionNumber: this.receptionNumber
      }

      cookingInstFlgUpdate(param).then(response => {
        if (response.status === 200) {
          // 打印账票
          this.cookingInstSlipPrt(this.queryParam, item)
          this.$msgUtil.messageNew('I_00030', message, this)
        } else if (response.status === 601) {
          this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
        } else if (response.status === 602) {
          this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
        }
      })
    },
    cookingInstSlipPrt(queryParam, itemData) {
      const param = {
        seatLevel: this.seatLevel,
        seatNumber: this.seatNumber,
        seatPeople:
          parseInt(queryParam.mainSeat.seatPeopleMan ? queryParam.mainSeat.seatPeopleMan : 0) +
          parseInt(queryParam.mainSeat.seatPeopleWoman ? queryParam.mainSeat.seatPeopleWoman : 0) +
          parseInt(queryParam.mainSeat.seatPeopleChildren ? queryParam.mainSeat.seatPeopleChildren : 0),
        slipList: [itemData]
      }

      cookingInstSlipPrt(param).then(response => {
        if (response.status === 200) {
        } else if (response.status === 601) {
          this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
        } else if (response.status === 602) {
          this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
        }
      })
    },
    // 关闭窗口
    onLoad() {}
  }
}
</script>
<style scoped lang="scss">
@import '@/styles/variables.scss';

.order-list {
  margin-bottom: 125px;

  .time-title {
    height: 34px;
    line-height: 34px;
    padding-left: 24px;
    background-image: linear-gradient(to right, $orderLinear1, $orderLinear2);
  }

  .title {
    font-size: $normalSize;
    color: rgba($color: $white, $alpha: 0.8);
  }

  .txt-light {
    color: $subTitleColor;
  }

  .left {
    .txt-light {
      margin-top: 2px;
    }
    .txt-light:last-child {
      margin-top: 5px;
    }
  }

  .txt-light.num {
    width: 60px;
    height: 30px;
    line-height: 28px;
    background: rgba($color: $white, $alpha: 0.1);
    padding: 0 5px;
    box-sizing: border-box;
  }

  .price {
    color: $priceColor;
    margin-left: 20px;
  }

  .price-red {
    color: $red;
  }

  .order-con {
    padding: 10px 24px;
    display: flex;
    justify-content: space-between;
    align-items: center;
    border-top: 3px transparent solid;
    border-image: linear-gradient(to right, $orderLinear1, $orderLinear2) 1 10;

    .total-price {
      // display: flex;
      // flex-direction: column;
      // justify-content: center;
      position: relative;
      width: 78px;

      > div {
        text-align: right;
        width: 100%;
      }

      .price {
        margin-left: 0;
      }

      .pop-txt {
        width: 25px;
        left: auto;
        top: 56px;
        right: 0;
        text-align: center;
        float: right;
      }
    }

    .left {
      padding-right: 75px;
    }

    .left > div:nth-child(2) {
      overflow: hidden;
      display: -webkit-box;
      -webkit-box-orient: vertical;
      -webkit-line-clamp: 2;
    }
  }

  > div > div:nth-child(2) {
    border-top: 0;
  }
}
.pop {
  width: 90%;
  padding: 20px;
  background: rgba($color: $white, $alpha: 0.8);
  box-sizing: border-box;
  color: $cartBgColor;
  border-radius: 10px;

  .remarks-wrap {
    position: relative;
    height: 35px;
    margin: 20px 0;
  }

  .remarks {
    width: 75%;
    height: 35px;
    position: absolute;
    left: 50%;
    top: 50%;
    transform: translate(-50%, -50%);
    line-height: 35px;
    padding: 4px;
    border-radius: 3px;
    border: 1px solid rgba($color: $white, $alpha: 0.8);
    box-sizing: border-box;
  }

  .btn {
    display: flex;
    justify-content: center;
    align-items: center;

    button {
      margin: 0 30px;
    }
  }
}

.order-total {
  position: fixed;
  padding: 0 22px;
  bottom: 72px;
  width: 100%;
  height: 55px;
  line-height: 55px;
  box-sizing: border-box;
  background-image: linear-gradient(to right, $orderSureLinear1, $orderSureLinear2);
  text-align: right;
  font-size: $titleSize;

  .txt {
    font-size: $smallSize;
  }

  > span:first-child {
    margin-right: 28px;
  }

  > span:nth-child(2) {
    margin-right: 5px;
  }

  > span:nth-child(3) {
    margin-right: 20px;
  }
}
</style>
<style lang="scss">
@import '@/styles/variables.scss';
</style>
