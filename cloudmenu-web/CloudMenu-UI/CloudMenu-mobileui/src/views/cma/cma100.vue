<template>
  <div ref="sellineName" @click="closeSel">
    <van-row v-if="message !== null && message !== ''" class="error-message">
      {{ message }}
    </van-row>
    <van-list v-model="loading" :finished="finished" :finished-text="emptyText" class="list">
      <van-cell v-for="(item, index) in list" :key="index" class="product-info">
        <template #title>
          <van-row>
            <!-- 1.有图上架商品 item.preparationFlg == 'OFF'是已上架，ON是未上架 image是否有图 :src="process.VUE_APP_BASE_API + '/file/getImage/' + item.image"-->
            <!-- 2.无图上架商品-->
            <van-col
              v-if="item.image != ''"
              style="width:120px;"
              span="8"
              class="list-l"
              @click.stop="chooseFood($event, item)"
            >
              <!-- // TODO 这个路劲要在mock数据里面写 -->
              <!-- <van-image width="110" height="110" :src="require('@/assets' + item.image)" /> -->
              <van-image width="110" height="110" :src="imgUrl + item.image" />
              <van-row class="layer">No.{{ item.menuOrderNumber }}</van-row>
              <!-- 未上架时加遮罩 -->
              <van-row v-if="item.preparationFlg === 'ON'" class="preparation">
                準備中
              </van-row>
            </van-col>

            <!-- 已上架和未上架分别有图分15份，无图份24份 -->
            <van-col
              :style="item.image != '' && item.image != null ? 'width:calc(100% - 120px);' : 'width:calc(100%);'"
              class="list-r"
            >
              <van-row class="food-txt">
                <van-row class="food-txt">
                  <span @click.stop="chooseFood($event, item)">
                    <i v-if="item.pickupFlg === 'ON'" />
                    {{ item.nameCh }}
                  </span>
                </van-row>
                <van-row>
                  <span class="desc" @click.stop="chooseFood($event, item)">
                    {{ item.nameJp }}
                  </span>
                </van-row>
                <van-row gutter="24">
                  <span
                    v-if="
                      item.offertimeFrom !== '' &&
                        item.offertimeTo !== '' &&
                        item.offertimeFrom !== null &&
                        item.offertimeTo !== null
                    "
                    @click.stop="chooseFood($event, item)"
                  >
                    {{ item.offertimeFrom + '~' + item.offertimeTo + 'のみ' }}
                    <b class="appointment">{{ item.remarks }}</b>
                  </span>
                  <span v-else @click.stop="chooseFood($event, item)">{{ item.remarks }}</span>
                </van-row>
                <van-row gutter="24">
                  <van-col span="24">
                    <span @click.stop="chooseFood($event, item)">
                      注文可能数：{{
                        item.limitedKbn === '001' || item.limitedKbn === '002' ? item.limitedQuantity : ''
                      }}
                    </span>
                  </van-col>
                </van-row>
                <van-row gutter="24">
                  <van-col span="12" style="padding-right:0px;">
                    <van-checkbox
                      v-if="item.setExistenceFlg === '0'"
                      v-model="item.orderTakeoutFlg"
                      shape="square"
                      class="checkBox"
                      @click="inOutChange(item)"
                    >
                      持ち帰り
                    </van-checkbox>
                  </van-col>
                  <van-col span="12" style="padding-left:0px;">
                    <span class="price" @click.stop="chooseFood($event, item)">
                      {{ item.setExistenceFlg === '1' ? '（セット品）' : '￥' + formatAmount(item.price) }}
                    </span>
                  </van-col>
                </van-row>
              </van-row>
            </van-col>
          </van-row>
        </template>
      </van-cell>
    </van-list>
    <van-popup v-model="showDialog" class="order-remarks">
      <van-row class="title">
        {{ orderInfo.nameCh }}
      </van-row>
      <van-row class="subtitle">
        厨房への備考情報を選択入力してください
      </van-row>
      <van-row class="select-down">
        <van-row class="select-input">
          <input v-model="orderInfo.orderRemarks" type="text" @click.stop="searchInfo">
          <i class="fa fa-caret-down" aria-hidden="true" />
        </van-row>

        <ul ref="selectList">
          <li v-for="(item, index) in items" :key="index" @click.stop="selectCurrent(index, $event)">
            <span>{{ item.text }}</span>
          </li>
        </ul>
      </van-row>
      <van-row v-if="orderInfo.setExistenceFlg == '1'" class="subtitle">商品セットを選択してください</van-row>
      <van-row v-if="orderInfo.setExistenceFlg == '1'" class="remarks-info">
        <van-radio-group v-model="chooseSetMeal">
          <van-radio v-for="(proInfo, index) in orderInfo.setMealList" :key="index" :name="proInfo" class="flex-radio">
            <van-row class="radio-item">
              <van-col span="14">
                <van-row>{{ proInfo.setName }}</van-row>
                <van-row>商品注文可能数:{{ proInfo.limitedQuantity }}</van-row>
              </van-col>
              <van-col span="10" class="fr">
                <van-row class="fr">{{ '￥' + formatAmount(proInfo.price) }}</van-row>
                <van-row class="fr">
                  <van-checkbox
                    ref="checkboxes"
                    v-model="proInfo.orderTakeoutFlg"
                    class="checkBox"
                    shape="square"
                    @click="inOutChange(proInfo)"
                  >
                    持ち帰り
                  </van-checkbox>
                </van-row>
              </van-col>
            </van-row>
          </van-radio>
        </van-radio-group>
      </van-row>

      <van-row class="row-btn">
        <van-col span="12">
          <van-button type="default" class="cancel fl" @click.stop="cancel">キャンセル</van-button>
        </van-col>
        <van-col span="12"><van-button type="primary" class="confirm fr" @click.stop="confirm">OK</van-button></van-col>
      </van-row>
    </van-popup>
  </div>
</template>
<script>
import { searchKuBun } from '@/api/common'
import { setShoppingCart } from '@/utils/auth'
import { selectShohinCChoiceData, selectShohinBChoiceData, serachCTaxKubun } from '@/api/cma/cma100'
import dateFormat from '@/utils/Date'
import defaultSettings from '@/settings'
export default {
  name: 'CommodityOrder',
  data() {
    return {
      message: '',
      showDialog: false,
      list: [],
      loading: false,
      finished: true,
      title: '厨房への備考情報がある場合は入力してください。',
      orderInfo: {
        menuLineNumber: null,
        menuOrderNumber: null,
        number: null,
        nameCh: null,
        nameJp: null,
        seatNumber: null,
        seatName: null,
        price: null,
        priceOld: null,
        taxKbn: null,
        offertimeFrom: null,
        offertimeTo: null,
        cookingtime: null,
        pickupFlg: null,
        setExistenceFlg: null,
        limitedKbn: null,
        image: null,
        preparationFlg: null,
        orderVoucherNumber: null,
        ordernumberOfPeople: null,
        remarks: null,
        limitedQuantity: null,
        typeKbn: null,
        setMealList: [],
        orderQuantity: 1,
        orderSlipFlg: false,
        orderRemarks: null,
        orderTakeoutFlg: false // 账票复选框
      },
      // 下拉
      option: [],
      // 选择套餐几人份
      chooseSetMeal: {
        orderQuantity: 1
      },
      // 临时领域
      queryParam: {
        // 选择中座位集合
        selectedSeatList: [],
        // 选择中有无座位
        selectedSeatOnoff: '0',
        // 坐席选择mode
        seatSelectMode: '0',
        // 商品menu分类表示顺序
        menuLineNumber: 0,
        // 商品menu注文番号
        menuOrderNumber: '',
        // 受付区分
        receptionKbn: '',
        // 坐席集合
        seatList: []
      },
      ctaxList: [], // 税率集合
      taxPercentIn: null, // 店内税率
      taxPercentOut: null // 外带税率
    }
  },
  computed: {
    imgUrl: function() {
      var imgUrl = `${process.env.VUE_APP_BASE_API}/file/getImage/${defaultSettings.storeNumber}/`
      return imgUrl
    },
    // 过滤方法
    items: function() {
      var _search = this.orderInfo.orderRemarks
      if (_search) {
        // 不区分大小写处理
        var reg = new RegExp(_search, 'ig')
        // es6 filter过滤匹配，有则返回当前，无则返回所有
        return this.option.filter(function(e) {
          // 匹配所有字段
          return Object.keys(e).some(function(key) {
            return e[key].match(reg)
          })
          // 匹配某个字段
          //  return e.name.match(reg);
        })
      }
      return this.option
    },
    emptyText: function() {
      return this.list.length === 0 ? 'なし' : ''
    }
  },
  created() {
    if (
      sessionStorage.getItem('seatInfo') &&
      sessionStorage.getItem('seatInfo') !== null &&
      sessionStorage.getItem('seatInfo') !== undefined
    ) {
      const queryParam = JSON.parse(sessionStorage.getItem('seatInfo'))
      this.queryParam = queryParam
      this.init()
    }
    this.searchKuBun()
    this.serachCTaxKubun()
  },
  methods: {
    // 初始化根据条件查询
    init() {
      const MenuOrderNumber = this.queryParam.menuOrderNumber
      const MenuLineNumber = this.queryParam.menuLineNumber
      if (MenuOrderNumber && MenuOrderNumber !== '') {
        selectShohinCChoiceData({ menuOrderNumber: MenuOrderNumber }).then(response => {
          this.searchOk(response)
        })
      } else if (MenuLineNumber !== '') {
        selectShohinBChoiceData({ menuLineNumber: MenuLineNumber }).then(response => {
          this.searchOk(response)
        })
      }
    },

    searchOk(response) {
      if (response.status === 200) {
        const list = response.shohinList
        // eslint-disable-next-line no-unused-vars
        let obj = {}
        const newList = []
        let setNumberList = []
        for (var i = 0; i < list.length; i++) {
          var item = list[i]
          item.orderTakeoutFlg = false
          item.orderSlipFlg = false
          item.orderRemarks = null
          item.orderQuantity = 1
          setNumberList.push({
            setNumber: item.setNumber,
            setName: item.setName,
            price: item.price,
            priceOld: item.price,
            limitedQuantity: item.limitedQuantity,
            orderTakeoutFlg: false
          })
          if (item.limitedKbn === '001' || item.limitedKbn === '002') {
            item.limitedQuantityMain = item.limitedQuantity
          } else {
            item.limitedQuantityMain = ''
          }

          if (i < list.length - 1 && list[i].number !== list[i + 1].number) {
            list[i].setMealList = setNumberList
            list[i].priceOld = list[i].price
            newList.push(list[i])
            setNumberList = []
          }
          obj = list[i]
        }

        list[list.length - 1].setMealList = setNumberList
        newList.push(list[list.length - 1])

        this.list = newList
      } else if (response.status === 601) {
        this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
      } else if (response.status === 602) {
        this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
      }
    },
    searchKuBun() {
      const param = { categoryClassNumber: '071' }
      searchKuBun(param).then(response => {
        for (let i = 0; i < response.categoryList.length; i++) {
          const item = {}
          item.value = response.categoryList[i].categoryKbn
          item.text = response.categoryList[i].categoryKbnName
          this.option.push(item)
        }
      })
    },
    serachCTaxKubun() {
      serachCTaxKubun().then(response => {
        if (response.status === 200) {
          // 税率列表取得
          this.ctaxList = response.ctaxList
          for (let index = 0; index < this.ctaxList.length; index++) {
            // 店内税率
            if (this.ctaxList[index].taxKbn === '010') {
              this.taxPercentIn = this.ctaxList[index].taxPercent
            }
            // 持ち帰り税率
            if (this.ctaxList[index].taxKbn === '020') {
              this.taxPercentOut = this.ctaxList[index].taxPercent
            }
          }
        } else if (response.status === 601) {
          this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
        } else if (response.status === 602) {
          this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
        }
      })
    },
    inOutChange(item) {
      if (item.orderTakeoutFlg) {
        if (item.taxKbn === '020') {
          item.price = item.priceOld
        } else {
          item.price = Math.floor(
            (
              Math.floor(
                (item.priceOld / (1 + parseFloat(this.taxPercentIn))) * (1 + parseFloat(this.taxPercentOut)) * 100
              ) / 100
            ).toFixed(0)
          )
        }
      } else {
        item.price = item.priceOld
      }
    },
    // 行点击事
    chooseFood($event, item) {
      this.message = ''
      if (item.preparationFlg === 'ON') {
        this.$msgUtil.messageNew('E_00030', '', this)
        return
      }
      if (item.setExistenceFlg === '0') {
        if (item.limitedQuantity === '0' || item.limitedQuantity === 0) {
          this.$msgUtil.messageNew('E_00180', '', this)
          return
        }
      }
      this.chooseSetMeal = {}
      if (!item.offertimeFrom || !item.offertimeTo) {
        this.chooseFood2(item)
        return
      }
      const beginTime = new Date()
      beginTime.setHours(item.offertimeFrom ? item.offertimeFrom.substring(0, 2) : '')
      beginTime.setMinutes(item.offertimeFrom ? item.offertimeFrom.substring(3, 5) : '')
      const endTime = new Date()
      endTime.setHours(item.offertimeTo ? item.offertimeTo.substring(0, 2) : '')
      endTime.setMinutes(item.offertimeTo ? item.offertimeTo.substring(3, 5) : '')
      const curDate = new Date()
      if (curDate < beginTime || curDate > endTime) {
        this.$msgUtil
          .messageBox('W_00010', '')
          .then(() => {
            this.chooseFood2(item)
          })
          .catch(() => {
            return
          })
      } else {
        this.chooseFood2(item)
      }
    },
    chooseFood2(item) {
      this.orderInfo = item
      // セット品有無フラグ='1'（あり）
      if (this.orderInfo.setExistenceFlg === '1') {
        if (item.setMealList !== undefined) {
          for (var i = 0; i < item.setMealList.length; i++) {
            item.setMealList[i].orderDatetime = dateFormat('YYYYmmddHHMMSS', new Date())
            // TODO 注文明細番号 没有字段名
            item.setMealList[i].orderQuantity = 1
            item.setMealList[i].orderTakeoutFlg = false
            item.setMealList[i].orderRemarks = null
          }
          // 默认选中套餐的第一个
          this.chooseSetMeal = item.setMealList[0]
          item.setMealList[0].orderRemarks = null
        }
      }
      this.showDialog = true
    },
    confirm() {
      if (this.orderInfo.setExistenceFlg === '0') {
        //
      } else if (this.orderInfo.setExistenceFlg === '1') {
        if (!this.chooseSetMeal.price) {
          this.showDialog = false
          this.$msgUtil.messageNew('E_00190', '', this)
          return
        }
        if (this.chooseSetMeal.limitedQuantity === 0) {
          this.$msgUtil.messageNew('E_00180', '', this)
          this.showDialog = false
          return
        }

        this.orderInfo.setNumber = this.chooseSetMeal.setNumber
        this.orderInfo.setName = this.chooseSetMeal.setName

        this.orderInfo.price = this.chooseSetMeal.price
        this.orderInfo.orderTakeoutFlg = this.chooseSetMeal.orderTakeoutFlg
      }

      setShoppingCart(this.orderInfo)
      this.$msgUtil.messageNew('I_00020', '', this)
      this.showDialog = false
      return
    },

    cancel() {
      this.showDialog = false
    },

    // 选中当前检索项
    selectCurrent(index, event) {
      var menuText = event.currentTarget.innerText
      this.orderInfo.orderRemarks = menuText
      if (this.$refs.selectList) {
        this.$refs.selectList.style.display = 'none'
      }
    },
    // 输入框输入事件
    searchInfo() {
      this.$refs.selectList.style.display = 'block'
    },
    closeSel(event) {
      const currentCli = this.$refs.sellineName
      // console.log(currentCli.contains)
      if (currentCli && this.$refs.selectList) {
        // if (!currentCli.contains(event.target)) {
        // 点击到了id为sellineName以外的区域，隐藏下拉框
        this.$refs.selectList.style.display = 'none'
        // }
      }
    }
  }
}
</script>

<style lang="scss" scoped>
@import '@/styles/variables.scss';

.order-remarks {
  max-height: 80%;

  .row-btn {
    justify-content: space-between;

    button {
      width: 105px;
      height: 40px;
      line-height: 40px;
      color: $white;
      border: 0;
      border-radius: 5px;
    }

    div:first-child button {
      background: $inputBorder;
    }

    div:last-child button {
      background-image: linear-gradient(to right, $linearBorder2, $linearBorder1) !important;
    }
  }

  .remarks-info {
    .radio-item {
      display: flex;
      align-items: center;
    }
  }
}
</style>

<style lang="scss">
@import '@/styles/variables.scss';

// 单选
.flex-radio {
  padding: 10px 6px 10px 6px;
  background: rgba($color: $white, $alpha: 0.1);
  margin-bottom: 10px;

  .van-radio__label {
    color: $mainDialogColor;
  }

  .van-radio__label {
    width: 100%;
  }

  .radio-item {
    > .van-col > .van-row:first-child {
      margin-bottom: 10px;
    }

    > .van-col:last-child {
      float: right;
      display: flex;
      flex-direction: column;

      .fr {
        margin-left: auto;
      }
    }

    .van-checkbox__label {
      color: $mainDialogColor;
    }
  }
}
</style>
