<template>
  <div>
    <van-row v-if="message !== null && message !== ''" class="error-message">
      {{ message }}
    </van-row>
    <van-list v-model="loading" :finished="finished" :finished-text="emptyText" class="list">
      <van-cell v-for="(item, index) in resultlist" :key="index" class="product-info">
        <template #title>
          <van-row>
            <van-col v-if="item.image !== '' && item.image !== null" style="width:120px;" class="list-l">
              <van-image width="110" height="110" :src="imgUrl + item.image" />
              <van-row class="layer">No.{{ item.menuOrderNumber }}</van-row>
            </van-col>
            <van-col
              :style="item.image != '' && item.image != null ? 'width:calc(100% - 120px);' : 'width:calc(100%);'"
              class="list-r"
            >
              <van-row class="food-txt">
                <span>
                  <!-- 是否推荐 -->
                  <i v-if="item.pickupFlg === 'ON'" />
                  {{ item.nameCh }}
                </span>
              </van-row>
              <van-row>
                <span class="desc">
                  {{ item.nameJp }}
                </span>
              </van-row>
              <van-row>
                <van-col>
                  <span
                    v-if="
                      (item.offertimeFrom !== null && item.offertimeFrom !== '') ||
                        (item.offertimeTo !== null && item.offertimeTo !== '')
                    "
                  >
                    {{ item.offertimeFrom + '~' + item.offertimeTo }} のみ
                  </span>
                </van-col>
                <van-col>
                  <span v-if="item.setName != null && item.setName !== ''" style="padding-left:10px;padding-right:5px;">
                    |
                  </span>
                  <span v-if="item.setName != null && item.setName !== ''">{{ item.setName }}</span>
                </van-col>
              </van-row>
              <van-row gutter="24" class="notes">
                <van-col style="padding-right:0px;" span="10">
                  <span class="fl">
                    注文可能数:{{ item.limitedKbn === '001' || item.limitedKbn === '002' ? item.limitedQuantity : '' }}
                  </span>
                </van-col>
                <van-col style="padding-left:0px;" span="14">
                  <van-row style="width:145px;background-color:#404040;" class="fr">
                    <van-col style="padding-right:0px;padding-left:8px;" span="10">
                      <span v-if="item.orderTakeoutFlg" style="color:#c6a26c;">
                        持ち帰り
                      </span>
                    </van-col>
                    <van-col style="padding-right:5px;" span="14">
                      <van-checkbox ref="checkboxes" v-model="item.orderSlipFlg" class="checkBox fr" shape="square">
                        <span v-if="!item.orderSlipFlg">
                          伝票出力
                        </span>
                        <span v-if="item.orderSlipFlg" style="color:#c6a26c;">
                          伝票出力
                        </span>
                      </van-checkbox>
                    </van-col>
                  </van-row>
                </van-col>
              </van-row>

              <van-row class="stepper">
                <van-row gutter="24">
                  <van-col span="10" style="padding-right: 0px;">
                    <van-stepper
                      v-model="item.orderQuantity"
                      input-width="30px"
                      button-size="25px"
                      :disable-input="true"
                      min="0"
                      :async-change="true"
                      integer
                      @plus="add(item)"
                      @minus="reduce(item, index)"
                    />
                  </van-col>
                  <van-col span="4">
                    <span
                      v-if="item.orderRemarks != null && item.orderRemarks != ''"
                      class="pop-txt"
                      @click="show(item)"
                    >
                      ...
                    </span>
                  </van-col>
                  <van-col span="10">
                    <span class="price">
                      {{ '￥' + formatAmount(item.price) }}
                    </span>
                  </van-col>
                </van-row>
              </van-row>
            </van-col>
          </van-row>
        </template>
      </van-cell>
    </van-list>

    <van-row class="total-info">
      <van-row class="left fl">
        <!-- <van-row>注文内容を確定し、調理開始します。</van-row> -->
        <van-row class="info">
          <span class="txt">計</span>
          <span class="txt">{{ total }}</span>
          &nbsp;
          <span class="txt-com">個</span>
          <span class="txt">
            <i>￥</i>
            {{ formatAmount(totalPrice) }}
          </span>
        </van-row>
      </van-row>
      <button class="btn fr" :disabled="total === 0" @click="sumbit">注文確定</button>
    </van-row>

    <!-- 圆弹出框 -->
    <van-popup v-model="showRemarks" class="order-remarks">
      <van-row class="title">
        {{ orderRemarksItem.nameCh }}
      </van-row>

      <van-field v-model="orderRemarksItem.orderRemarks" rows="2" autosize type="textarea" readonly />

      <van-row class="row-btn">
        <van-button type="default" class="cancel fl" @click="showRemarks = false">OK</van-button>
      </van-row>
    </van-popup>
  </div>
</template>
<script>
import { insertOrderData, cookingInstSlipPrt } from '@/api/cma/cma110'
import { getShoppingCart, setShoppingCart, removeShoppingCart, clearShoppingCart } from '@/utils/auth'
import defaultSettings from '@/settings'
export default {
  name: 'OrderDecision',
  data() {
    return {
      resultlist: [],
      total: 0,
      totalPrice: 0,
      seatLevel: '',
      seatNumber: '',
      receptionBranchNumber: '',
      receptionNumber: '',
      message: '',
      loading: false,
      finished: true,
      disabled: false,
      orderRemarks: '',
      showRemarks: false,
      orderRemarksItem: {}
    }
  },
  computed: {
    imgUrl: function() {
      var imgUrl = `${process.env.VUE_APP_BASE_API}/file/getImage/${defaultSettings.storeNumber}/`
      return imgUrl
    },
    emptyText: function() {
      return this.resultlist.length === 0 ? 'なし' : ''
    }
  },
  created() {
    // 初始化
    this.getList()
  },
  methods: {
    // 初始化取值
    getList() {
      this.resultlist = []
      const result = getShoppingCart() // 获取所有数据
      if (result === null) {
        return
      }

      if (result.orderList !== undefined) {
        const list = result.orderList
        if (list && list !== null && list.length === 0) {
          this.$msgUtil.messageNew('E_00040', '', this)
        } else {
          for (let index = 0; index < list.length; index++) {
            const element = list[index]

            element.orderSlipFlg = false
            this.total++ // 合计个数
            this.totalPrice += element.price // 合计价格
            this.resultlist.push(element)
          }
        }
        if (this.total === null) {
          this.total = 0
        }
        if (this.totalPrice === null) {
          this.totalPrice = 0
        }
      }
    },
    add(item) {
      // 加算後の個数＞注文可能数
      if (item.limitedKbn === '001' || item.limitedKbn === '002') {
        if (item.orderQuantity > item.limitedQuantity) {
          this.$msgUtil.messageNew('E_00180', null, this)
          item.orderQuantity--
          return false
        }
      }
      let foodPrice = 0
      if (item.setExistenceFlg === '1') {
        foodPrice = item.price
      } else {
        foodPrice = item.price
      }
      setShoppingCart(item)
      this.total++
      this.totalPrice += foodPrice
    },
    reduce(item, index) {
      let foodPrice = 0
      if (item.setExistenceFlg === '1') {
        foodPrice = item.price
      } else {
        foodPrice = item.price
      }
      if (item.orderQuantity === 0) {
        this.$msgUtil
          .messageBox('W_00020')
          .then(() => {
            this.total--
            this.totalPrice -= foodPrice
            removeShoppingCart(item.number)
            this.resultlist.splice(index, 1)
          })
          .catch(() => {
            item.orderQuantity++
          })
      } else {
        this.total--
        this.totalPrice -= foodPrice
        setShoppingCart(item)
      }
    },
    show(item) {
      this.showRemarks = true
      this.orderRemarks = item.orderRemarks
      this.orderRemarksItem = item
    },
    // 提交
    sumbit() {
      this.message = null
      let queryParam = {}
      if (
        sessionStorage.getItem('seatInfo') &&
        sessionStorage.getItem('seatInfo') !== null &&
        sessionStorage.getItem('seatInfo') !== undefined
      ) {
        queryParam = JSON.parse(sessionStorage.getItem('seatInfo'))
        this.seatLevel = queryParam.mainSeat.seatLevel
        this.seatNumber = queryParam.mainSeat.seatNumber
        this.receptionBranchNumber = queryParam.mainSeat.receptionBranchNumber
        this.receptionNumber = queryParam.mainSeat.receptionNumber
      }
      const orderCart = []
      for (let index = 0; index < this.resultlist.length; index++) {
        const element = this.resultlist[index]
        var orderNumberDetails = (index + 1).toString().padStart(3, '0')
        orderCart.push({
          orderDatetime: '',
          orderNumber: orderNumberDetails,
          productNumber: element.number,
          productSetNumber: element.setMealList[0].setNumber,
          productNameCh: element.nameCh,
          productSetName: element.setMealList[0].setName,
          productTypeKbn: element.typeKbn,
          orderQuantity: element.orderQuantity,
          orderPrice: element.price,
          orderTakeoutFlg: element.orderTakeoutFlg === true ? '1' : '0',
          orderRemarks: element.orderRemarks,
          orderSlipFlg: element.orderSlipFlg === true ? '1' : '0'
        })
      }
      const param = {
        seatLevel: this.seatLevel,
        seatNumber: this.seatNumber,
        receptionBranchNumber: this.receptionBranchNumber,
        receptionNumber: this.receptionNumber,
        orderCart: orderCart
      }
      insertOrderData(param).then(response => {
        if (response.status === 200) {
          clearShoppingCart()
          var message = response.orderList[0].orderVoucherNumber
          // 打印账票
          this.cookingInstSlipPrt(queryParam, response)
          this.$msgUtil.messageNew('I_00030', message, this)
        } else if (response.status === 601) {
          this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
        } else if (response.status === 602) {
          this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
        }
      })
    },
    cookingInstSlipPrt(queryParam, response) {
      const orderCart = []
      for (let index = 0; index < response.orderList.length; index++) {
        const element = response.orderList[index]
        if (element.orderSlipFlg === '1') {
          element.sysytemDatetime = new Date()
          orderCart.push(element)
        }
      }
      const param = {
        seatLevel: this.seatLevel,
        seatNumber: this.seatNumber,
        seatPeople:
          parseInt(queryParam.mainSeat.seatPeopleMan ? queryParam.mainSeat.seatPeopleMan : 0) +
          parseInt(queryParam.mainSeat.seatPeopleWoman ? queryParam.mainSeat.seatPeopleWoman : 0) +
          parseInt(queryParam.mainSeat.seatPeopleChildren ? queryParam.mainSeat.seatPeopleChildren : 0),
        slipList: orderCart
      }
      cookingInstSlipPrt(param).then(response => {
        if (response.status === 200) {
          this.getList()
          this.$router.push({ path: '/Employee/CommodityOrder' })
        } else if (response.status === 601) {
          this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
        } else if (response.status === 602) {
          this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
        }
      })
    }
  }
}
</script>
<style scoped lang="scss">
@import '@/styles/variables.scss';
.list {
  .desc {
    display: block;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
  }
}

// .notes {
//   overflow: hidden;
//   .fr {
//     width: 80px;
//     position: relative;
//   }
// }

.total-info {
  width: 100%;
  padding: 14px 20px;
  background-image: linear-gradient(to right, $linearBorder2, $linearBorder1);
  // display: flex;
  // justify-content: space-between;
  // align-items: center;
  position: fixed;
  bottom: 78px;
  left: 0;
  box-sizing: border-box;
  line-height: 38px;

  .txt {
    font-size: $titleSize;
  }

  .left > div:first-child {
    font-size: $moreSmallSize;
    // margin-bottom: 17px;
  }

  .info .txt:first-child {
    margin-right: 14px;
  }

  .info .txt:last-child {
    i {
      font-style: normal;
      font-size: $smallSize;
    }
  }

  .txt-com {
    font-size: $smallSize;
    margin-right: 20px;
  }

  .btn {
    height: 38px;
    width: 106px;
    background-color: $titleColor;
    border-radius: 18px;
    border: 0;
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
</style>

<style lang="scss">
@import '@/styles/variables.scss';
.list {
  margin-bottom: 170px;

  .stepper {
    // position: absolute;
    // left: 0;
    // bottom: 0;
    // width: 100%;

    margin-top: 5px;

    .van-stepper__input {
      color: $white;
      background-color: transparent;
      border: 1px solid rgba($color: $inputBorder, $alpha: 0.6);
      font-size: $titleSize;
    }

    .van-stepper__minus,
    .van-stepper__plus {
      color: $cartColor;
      background-color: transparent;
    }
  }

  .van-cell {
    line-height: 18px;
  }
}

// .van-dialog__header {
//   padding: 20px 20px 0;
//   text-align: left;
// }

.van-button--plain.van-button--info {
  color: $inputBorder;
}

.van-button--info {
  border: $cartBgColor;
}

.van-button--primary {
  background-color: $cartBgColor;
  border-color: $cartBgColor;
}
</style>
