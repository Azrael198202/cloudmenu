<template>
  <div
    dark
    class="grey darken-4 white--text"
    style="width: 1903px; border: 2px solid #ddd; user-select: none"
  >
    <!-- ***Header************************************************************************************************************** -->
    <v-container
      fluid
      class="text-h5 font-weight-blod"
      style="position: sticky; top: 0px"
    >
      <v-row no-gutters align="center" class="">
        <v-col cols="4" class="text-left pl-5" @click="routeBack()">
          <v-icon large class="mr-2" color="red">mdi-backburger</v-icon>{{SysConst.storeName}}
        </v-col>
        <v-col cols="4" class="text-center">会計レジ</v-col>
        <v-col cols="4" class="text-right pr-5">{{ nowStrJp }}</v-col>
      </v-row>
    </v-container>
    <!-- ***Body************************************************************************************************************** -->
    <v-container fluid class="" style="height: 926px">
      <v-row no-gutters style="height: 100%">
        <!-- ***Body-Left************************************************************************************************************** -->
        <v-col cols="4" class="text-h6">
          <v-container class="grey darken-3 rounded-lg h6">
            <v-row no-gutters align="center">
              <v-img
                max-height="45"
                max-width="45"
                src="/img/stuff.png"
                style="display: inline-block"
              ></v-img>
              <span>担当者</span>
              <v-spacer></v-spacer>
              <span>老上海</span>
            </v-row>
          </v-container>
          <v-container class="grey darken-3 rounded-lg mt-3">
            <v-row no-gutters align="center">
              <v-img
                max-height="45"
                max-width="45"
                src="/img/customers.png"
                style="display: inline-block"
              ></v-img>
              <span>顧客</span>
              <v-spacer></v-spacer>
              <span>{{ seatTakeoutRecpInfo.customRepresentName }}</span>
            </v-row>
            <v-row no-gutters align="center" class="pl-11 text-subtitle-1">
              <span>{{ seatTakeoutRecpInfo.seatMergeAll }}</span>
              <v-spacer></v-spacer>
              <span>{{ seatTakeoutRecpInfo.seatPelpleAll }}人</span>
            </v-row>
          </v-container>
          <v-container class="my-1">
            <v-row no-gutters align="center" class="text-subtitle-1">
              <div class="grey--text">注文一覧</div>
              <v-spacer></v-spacer>
              <div
                class="grey darken-3 rounded-pill px-3"
                @click="payAllOrder()"
              >
                すべて >>
              </div>
            </v-row>
          </v-container>
          <v-virtual-scroll
            :items="orderInfoList"
            height="668px"
            item-height="87"
          >
            <template v-slot:default="{ item }">
              <v-container
                class="grey darken-3 rounded-lg text-subtitle-2 pa-2"
              >
                <v-row no-gutters align="center">
                  <div class="mx-3 text-subtitle-1">
                    <div>
                      {{ item.orderVoucherDetails }}
                    </div>
                    <div v-if="item.paymentOver===1" class="text-caption red--text">
                      {{"会計済"}}
                    </div>
                    <div v-else class="text-caption">
                      {{"未会計"}}
                    </div>
                  </div>
                  <div class="mr-2" style="width: 260px">
                    <div class="text-subtitle-1">
                      <span>{{ item.productShowName }}</span>
                    </div>
                    <div class="text-caption grey--text">
                      {{ item.productShowNameJp }}
                    </div>
                    <div class="text-caption grey--text">
                      {{ item.productSetName }}
                    </div>
                    <!-- <div class="text-caption blue--text">
                      300円の割引券
                    </div> -->
                  </div>
                  <div class="mr-2 text-right" style="width: 45px">
                    {{ item.orderQuantity }}個
                  </div>
                  <div class="mr-2 text-right" style="width: 45px">
                    {{ item.taxRate }}%
                  </div>
                  <div class="mr-2 text-right" style="width: 100px">
                    <div class="">
                      {{ jpCurrencyFmt.format(item.orderAfterPrice) }}
                    </div>
                    <!-- <div class="red--text">
                    ￥ 112,300
                    </div> -->
                  </div>
                  <v-spacer></v-spacer>
                  <v-icon
                    v-if="item.casherPaySelected === 0"
                    large
                    dark
                    @click="item.casherPaySelected = 1"
                  >
                    mdi-arrow-right-drop-circle-outline
                  </v-icon>
                  <v-icon
                    v-if="item.casherPaySelected === 1"
                    large
                    dark
                    color="orange"
                    @click="item.casherPaySelected = 0"
                  >
                    mdi-arrow-left-drop-circle-outline
                  </v-icon>
                </v-row>
              </v-container>
            </template>
          </v-virtual-scroll>
        </v-col>
        <!-- ***Body-Center************************************************************************************************************** -->
        <v-col cols="4" class="px-5">
          <v-container
            class="
              white
              rounded-lg
              black--text
              text-caption
              px-8
              font-weight-bold
            "
            style="height: 840px; overflow-y: auto; user-select: none"
          >
            <v-row no-gutters justify="center" class="text-h6">
              {{ storeInfo.storeName }}
            </v-row>
            <v-row no-gutters justify="center">
              {{ storeInfo.storeAddress }}
            </v-row>
            <v-row no-gutters justify="center">
              {{ storeInfo.storeAddress2 }}
            </v-row>
            <v-row no-gutters justify="center">
              {{ storeInfo.storeTel }}
            </v-row>
            <v-row no-gutters>
              <v-col>{{ nowStrJp }}</v-col>
              <v-col class="text-right">{{
                seatTakeoutRecpInfo.seatMergeAll
              }}</v-col>
            </v-row>
            <v-row no-gutters>
              <v-col>No.{{ seatTakeoutRecpInfo.receptionNumber }}</v-col>
              <v-col class="text-right"
                >人数：{{ seatTakeoutRecpInfo.seatPelpleAll }}</v-col
              >
            </v-row>
            <v-row no-gutters class="mb-3">
              受付時間：{{ seatTakeoutRecpInfo.seatStartDateHm }}
            </v-row>
            <div class="lw-bdx" style="height: 495px; overflow-y: auto">
              <div v-for="(orderInfo, index) in orderInfoList" :key="index">
                <v-row
                  v-if="orderInfo.casherPaySelected === 1"
                  no-gutters
                  class="mt-2"
                >
                  <v-col cols="5">
                    <div>
                      {{ orderInfo.productShowName }}
                    </div>
                    <div class="grey--text">
                      {{ orderInfo.productShowNameJp }}
                    </div>
                    <div class="grey--text">
                      {{ orderInfo.productSetName }}
                    </div>
                    <!-- <div>300円の割引券</div> -->
                  </v-col>
                  <v-col cols="2" class="text-center">
                    {{ orderInfo.orderQuantity }}
                  </v-col>
                  <v-col cols="5" class="text-right">
                    <div>
                      {{ jpCurrencyFmt.format(orderInfo.orderAfterPrice) }}
                    </div>
                    <!-- <div class="white--text">0</div>
                    <div>￥ -300</div> -->
                  </v-col>
                </v-row>
              </div>
            </div>
            <v-divider class="my-2"></v-divider>
            <v-row no-gutters>
              <v-col>商品計</v-col>
              <v-col class="text-center">{{
                paymentTotalInfo.subTotalQuantity
              }}</v-col>
              <v-col class="text-right">{{
                jpCurrencyFmt.format(paymentTotalInfo.subTotalAmount)
              }}</v-col>
            </v-row>
            <v-row no-gutters>
              <v-col>サービス料</v-col>
              <v-col class="text-right">{{
                jpCurrencyFmt.format(paymentTotalInfo.serviceAmount)
              }}</v-col>
            </v-row>
            <v-row no-gutters>
              <v-col>小計</v-col>
              <v-col class="text-right">{{
                jpCurrencyFmt.format(paymentTotalInfo.subTotalAmount+paymentTotalInfo.serviceAmount)
              }}</v-col>
            </v-row>
            <v-row no-gutters>
              <v-col>値割引</v-col>
              <v-col class="text-right">
                <span>￥</span>
                <span v-if="paymentTotalInfo.discountAmount > 0">-</span>
                <span>{{numberFmt.format(paymentTotalInfo.discountAmount)}}</span>
              </v-col>
            </v-row>
            <v-divider class="my-2"></v-divider>
            <v-row no-gutters>
              <v-col>合計</v-col>
              <v-col class="text-right">{{
                jpCurrencyFmt.format(cmptTotalAmount)
              }}</v-col>
            </v-row>
            <v-row no-gutters>
              <v-col>内消費税</v-col>
              <v-col class="text-right">{{
                jpCurrencyFmt.format(cmptTotalTaxAmount)
              }}</v-col>
            </v-row>
          </v-container>
          <div>
            <v-row>
              <v-col>
                <v-btn dark block x-large class="mt-2 blue darken-3 text-h5"
                  @click="printCashierChit()">
                  レシート
                </v-btn>
              </v-col>
              <v-col>
                <v-btn dark block x-large class="mt-2 blue darken-3 text-h5"
                  @click="printCashierReceipt()">
                  領収書
                </v-btn>
              </v-col>
              <v-col>
                <v-btn dark block x-large class="mt-2 red darken-3 text-h5"
                  @click="paymentBtnClicked()">
                  会計
                </v-btn>
              </v-col>
            </v-row>
          </div>
        </v-col>
        <!-- ***Body-Right************************************************************************************************************** -->
        <v-col cols="4" class="">
          <v-container
            class="grey darken-3 px-6 rounded-lg"
            style="height: 100%"
          >
            <div class="lw-bdx" style="height: 450px">
              会計種類
              <v-row no-gutters class="grey darken-2">
                <v-col
                  cols="3"
                  v-for="(payGp, index) in paymentGpClassLst"
                  :key="index"
                >
                  <v-btn
                    dark
                    style="width: 120px"
                    class="darken-1 text-caption ma-2"
                    v-bind:class="paymentClassKbnCss(payGp.categoryClassNumber)"
                    @click="changePaymentClassKbn(payGp.categoryClassNumber)"
                  >
                    <v-icon v-if="payGp.categoryClassOptionValues" left>{{
                      payGp.categoryClassOptionValues
                    }}</v-icon>
                    {{ payGp.categoryClassName }}
                  </v-btn>
                </v-col>
              </v-row>
              <v-row no-gutters class="mt-2 grey darken-2">
                <v-col
                  cols="3"
                  v-for="(payKbn, index) in paymentClassLst"
                  :key="index"
                >
                  <v-btn
                    dark
                    class="darken-1 text-caption ma-2"
                    style="width: 120px; text-transform: none"
                    v-bind:class="paymentKbnCss(payKbn.categoryKbn)"
                    @click="paymentTotalInfo.paymentKbn = payKbn.categoryKbn"
                  >
                    <v-icon v-if="payKbn.categoryOptionValues" left>{{
                      payKbn.categoryOptionValues
                    }}</v-icon>
                    {{ payKbn.categoryKbnAbbreviation }}
                  </v-btn>
                </v-col>
              </v-row>
            </div>

            <div class="px-6">
              <div
                v-if="!inputDiscountEdit"
                dark
                class="grey rounded pa-3 mb-2 text-h6 text-center"
                style="height: 55px"
                @click="beginEditDiscount()"
              >
                <v-icon dark>mdi-pencil</v-icon>
                値割引登録
              </div>
              <div v-else>
                <v-row no-gutters align="center">
                  <v-col cols="2" class="text-center">
                    <v-icon large dark color="red" @click="cancelEditDiscount()"
                      >mdi-pencil-remove</v-icon
                    >
                  </v-col>
                  <v-col cols="5" class="pr-2">
                    <div
                      dark
                      class="rounded pa-3 mb-2 text-h5 text-right"
                      style="height: 55px"
                      v-bind:class="{
                        green: inputDiscountType === 1,
                        grey: inputDiscountType === 2,
                      }"
                      @click="inputDiscountType = 1"
                    >
                      {{ inputDiscountPercent }}%
                    </div>
                  </v-col>
                  <v-col cols="5" class="pl-2">
                    <div
                      dark
                      class="rounded pa-3 mb-2 text-h5 text-right"
                      style="height: 55px"
                      v-bind:class="{
                        green: inputDiscountType === 2,
                        grey: inputDiscountType === 1,
                      }"
                      @click="inputDiscountType = 2"
                    >
                      {{ jpCurrencyFmt.format(inputDiscountAmount) }}
                    </div>
                  </v-col>
                </v-row>
              </div>
              <v-row no-gutters class="py-2 text-center">
                <v-col
                  class="grey darken-2 red--text rounded text-h5 py-3 mr-2"
                  @click="eidtDiscount(0, 0)"
                >
                  C
                </v-col>
                <v-col
                  class="grey darken-2 rounded text-h5 py-3 ml-2"
                  @click="eidtDiscount(0.1, 0)"
                >
                  <v-icon color="red">mdi-backspace-outline</v-icon>
                </v-col>
              </v-row>
              <v-row no-gutters class="py-2 text-center text-h5">
                <v-col
                  class="grey darken-2 rounded py-3 mr-2"
                  @click="eidtDiscount(10, 7)"
                >
                  7
                </v-col>
                <v-col
                  class="grey darken-2 rounded py-3 mx-2"
                  @click="eidtDiscount(10, 8)"
                >
                  8
                </v-col>
                <v-col
                  class="grey darken-2 rounded py-3 ml-2"
                  @click="eidtDiscount(10, 9)"
                >
                  9
                </v-col>
              </v-row>
              <v-row no-gutters class="py-2 text-center text-h5">
                <v-col
                  class="grey darken-2 rounded py-3 mr-2"
                  @click="eidtDiscount(10, 4)"
                >
                  4
                </v-col>
                <v-col
                  class="grey darken-2 rounded py-3 mx-2"
                  @click="eidtDiscount(10, 5)"
                >
                  5
                </v-col>
                <v-col
                  class="grey darken-2 rounded py-3 ml-2"
                  @click="eidtDiscount(10, 6)"
                >
                  6
                </v-col>
              </v-row>
              <v-row no-gutters class="py-2 text-center text-h5">
                <v-col
                  class="grey darken-2 rounded py-3 mr-2"
                  @click="eidtDiscount(10, 1)"
                >
                  1
                </v-col>
                <v-col
                  class="grey darken-2 rounded py-3 mx-2"
                  @click="eidtDiscount(10, 2)"
                >
                  2
                </v-col>
                <v-col
                  class="grey darken-2 rounded py-3 ml-2"
                  @click="eidtDiscount(10, 3)"
                >
                  3
                </v-col>
              </v-row>
              <v-row no-gutters class="py-2 text-center text-h5">
                <v-col
                  class="grey darken-2 rounded py-3 mr-2"
                  @click="eidtDiscount(10, 0)"
                >
                  0
                </v-col>
                <v-col
                  class="grey darken-2 rounded py-3 mx-2"
                  @click="eidtDiscount(100, 0)"
                >
                  00
                </v-col>
                <v-col
                  class="green darken-2 rounded py-3 ml-2 text-h6"
                  @click="endEditDiscount()"
                >
                  決定
                </v-col>
              </v-row>
            </div>
          </v-container>
        </v-col>
      </v-row>
    </v-container>
    <!-- ***Dialog************************************************************************************************************** -->
    <v-dialog v-model="continueConfirmDialog" persistent max-width="290">
      <v-card>
        <v-card-title class="text-h5"> 合計確認 </v-card-title>
        <v-card-text>合計金額は0以下、処理は続きますが？</v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn
            color="red darken-1"
            text
            @click="continueConfirmDialog = false"
          >
            戻る
          </v-btn>
          <!-- @click="doPrint()" -->
          <v-btn color="green darken-1" text @click="methodsToRunWhenContinue">
            続き
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    <v-dialog v-model="chitXmlDialog" persistent max-width="400">
      <v-card>
        <v-card-title class="text-h5"> レシートと領収書の結果 </v-card-title>
        <v-card-text>
          {{chitXmlText}}
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn
            color="red darken-1"
            text
            @click="copyXmlToClipboard()"
          >
            コピー
          </v-btn>
          <v-btn
            color="red darken-4"
            text
            @click="chitXmlDialog=false"
          >
            閉じる
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>
<script>
import SysConst from '../lwUtils/SysConst'
import moment from 'moment'
const jpCurrencyFmt = new Intl.NumberFormat('ja-JP', {
  style: 'currency',
  currency: 'JPY'
})
const numberFmt = new Intl.NumberFormat()

export default {
  name: 'Home',
  components: {},
  data: () => ({
    SysConst: SysConst,
    // 日本金額フォマード
    jpCurrencyFmt: jpCurrencyFmt,
    numberFmt: numberFmt,
    // システム日時 eg 2021/05/17(月) 12:30
    nowStrJp: '',
    items: Array.from({ length: 40 }, (k, v) => v + 1),
    storeInfo: {},
    seatTakeoutRecpInfo: {},
    orderInfoList: [],
    paymentGpClassLst: [],
    paymentClassLstAll: [],
    paymentTotalInfo: {
      subTotalQuantity: 0,
      subTotalAmount: 0,
      discountType: 1,
      discountPercent: 0,
      discountAmount: 0,
      serviceAmount: 0,
      // totalAmount: 0,
      totalTaxAmount: 0,
      paymentClassKbn: '',
      paymentKbn: ''
    },
    inputDiscountEdit: false,
    inputDiscountType: 1,
    inputDiscountPercent: 0,
    inputDiscountAmount: 0,
    // ----------------------------------
    // マイナス金額時、続き確認ダイアログ
    continueConfirmDialog: false,
    printReceipt1: true, // レシート印刷
    printReceipt2: false, // 領収書印刷
    // 処理続くダイアログで「続き」時の処理
    methodsToRunWhenContinue: null,
    // ----------------------------------
    chitXmlDialog: false,
    chitXmlText: ''
  }),
  computed: {
    cmptTotalAmount () {
      return (
        this.paymentTotalInfo.subTotalAmount -
        Math.abs(this.paymentTotalInfo.discountAmount) +
        this.paymentTotalInfo.serviceAmount
      )
    },
    cmptTotalTaxAmount () {
      var totalTaxAmount = 0
      // for (var index = 0; index < this.orderInfoList.length; index++) {
      //   var orderInfo = this.orderInfoList[index]
      //   if (orderInfo.casherPaySelected === 1) {
      //     totalTaxAmount = totalTaxAmount + orderInfo.includedTaxAmount
      //   }
      // }

      // var seatServiceTaxPercent =
      //   this.seatTakeoutRecpInfo.seatServiceTaxPercent
      // var serviceAmount = this.paymentTotalInfo.serviceAmount
      // var serviceTextFee = Math.floor(
      //   serviceAmount - (serviceAmount * 100) / (100 + seatServiceTaxPercent)
      // )
      // totalTaxAmount = totalTaxAmount + serviceTextFee

      // var discountTaxPercent = this.seatTakeoutRecpInfo.discountTaxPercent
      // var discountAmount = this.paymentTotalInfo.discountAmount
      // var discountTextFee = Math.floor(
      //   discountAmount - (discountAmount * 100) / (100 + discountTaxPercent)
      // )
      // totalTaxAmount = totalTaxAmount - discountTextFee

      var taxMap = new Map()
      // 消費税ごと合計
      for (var index = 0; index < this.orderInfoList.length; index++) {
        var orderInfo = this.orderInfoList[index]
        if (orderInfo.casherPaySelected === 1) {
          if (taxMap.has(orderInfo.taxRate)) {
            const taxTotalAmount = taxMap.get(orderInfo.taxRate)
            taxMap.set(orderInfo.taxRate, taxTotalAmount + orderInfo.orderAfterPrice)
          } else {
            taxMap.set(orderInfo.taxRate, orderInfo.orderAfterPrice)
          }
        }
      }
      // サービス料
      const seatServiceTaxPercent =
        this.seatTakeoutRecpInfo.seatServiceTaxPercent
      const serviceAmount = this.paymentTotalInfo.serviceAmount
      if (taxMap.has(seatServiceTaxPercent)) {
        const taxTotalAmount = taxMap.get(seatServiceTaxPercent)
        taxMap.set(seatServiceTaxPercent, taxTotalAmount + serviceAmount)
      } else {
        taxMap.set(seatServiceTaxPercent, serviceAmount)
      }
      // 割引料
      const discountTaxPercent = this.seatTakeoutRecpInfo.discountTaxPercent
      const discountAmount = this.paymentTotalInfo.discountAmount
      if (taxMap.has(discountTaxPercent)) {
        const taxTotalAmount = taxMap.get(discountTaxPercent)
        taxMap.set(discountTaxPercent, taxTotalAmount - discountAmount)
      } else {
        taxMap.set(discountTaxPercent, discountAmount)
      }

      for (const [taxRate, amount] of taxMap) {
        var taxFee = Math.floor(amount - (amount * 100) / (100 + taxRate))
        totalTaxAmount = totalTaxAmount + taxFee
      }

      return totalTaxAmount
    },
    paymentClassLst () {
      const paymentClassKbn = this.paymentTotalInfo.paymentClassKbn
      const paymentClassLst = []
      for (let index = 0; index < this.paymentClassLstAll.length; index++) {
        const payClss = this.paymentClassLstAll[index]
        if (payClss.categoryClassNumber === paymentClassKbn) {
          paymentClassLst.push(payClss)
        }
      }

      return paymentClassLst
    }
  },
  watch: {
    orderInfoList: {
      handler (val, oldVal) {
        var subTotalQuantity = 0
        var subTotalAmount = 0
        var totalTaxAmount = 0
        for (var index = 0; index < this.orderInfoList.length; index++) {
          var orderInfo = this.orderInfoList[index]
          if (orderInfo.casherPaySelected === 1) {
            subTotalQuantity = subTotalQuantity + orderInfo.orderQuantity
            subTotalAmount = subTotalAmount + orderInfo.orderAfterPrice
            totalTaxAmount = totalTaxAmount + orderInfo.includedTaxAmount
          }
        }
        // 小計
        this.paymentTotalInfo.subTotalQuantity = subTotalQuantity
        this.paymentTotalInfo.subTotalAmount = subTotalAmount
        // サービス料
        var serviceAmount = Math.floor(
          (subTotalAmount * this.seatTakeoutRecpInfo.seatServicePercent) / 100
        )
        this.paymentTotalInfo.serviceAmount = serviceAmount

        var seatServicePercent = this.seatTakeoutRecpInfo.seatServicePercent
        var serviceTextFee = Math.floor(
          serviceAmount - (serviceAmount * 100) / (100 + seatServicePercent)
        )
        totalTaxAmount = totalTaxAmount + serviceTextFee
        // 合計
        this.paymentTotalInfo.totalTaxAmount = totalTaxAmount

        this.cancelEditDiscount()
      },
      deep: true
    }
  },
  mounted () {
    moment.locale('ja')
    this.nowStrJp = moment().format('llll')
    setInterval(this.refeshSysDateTime, 3500)
    this.getInitData()
  },
  methods: {
    refeshSysDateTime () {
      // システム日付
      this.nowStrJp = moment().format('llll')
    },
    routeBack () {
      this.$router.go(-1)
    },
    async getInitData () {
      var recpNumber = this.$router.history.current.params.recpnumber
      // var recpNumber = '20210617004'
      const response = await this.$lwHttp.postAsync(
        'cashier/selectReciptionInfo',
        { storeNumber: SysConst.storeNumber, SelectedRecpNumber: recpNumber }
      )
      this.storeInfo = response.storeInfo
      this.seatTakeoutRecpInfo = response.seatTakeoutRecpInfo

      this.orderInfoList.splice(0)
      this.orderInfoList.push(...response.orderInfoList)

      this.paymentGpClassLst.splice(0)
      this.paymentGpClassLst.push(...response.paymentGpClassLst)

      this.paymentClassLstAll.splice(0)
      this.paymentClassLstAll.push(...response.paymentClassLst)

      this.paymentTotalInfo = {
        subTotalQuantity: 0,
        subTotalAmount: 0,
        discountType: 1,
        discountPercent: 0,
        discountAmount: 0,
        serviceAmount: 0,
        totalTaxAmount: 0,
        paymentClassKbn: '',
        paymentKbn: ''
      }
      this.changePaymentClassKbn(this.paymentGpClassLst[0].categoryClassNumber)
    },
    payAllOrder () {
      for (let index = 0; index < this.orderInfoList.length; index++) {
        const orderInfo = this.orderInfoList[index]
        if (orderInfo.casherPaySelected === 0) {
          this.orderInfoList[index].casherPaySelected = 1
        }
      }
    },
    changePaymentClassKbn (paymentClassKbn) {
      this.paymentTotalInfo.paymentClassKbn = paymentClassKbn

      for (let index = 0; index < this.paymentClassLstAll.length; index++) {
        const payClss = this.paymentClassLstAll[index]
        if (payClss.categoryClassNumber === paymentClassKbn) {
          this.paymentTotalInfo.paymentKbn = payClss.categoryKbn
          return
        }
      }
    },
    beginEditDiscount () {
      this.inputDiscountAmount = Math.abs(this.paymentTotalInfo.discountAmount)
      this.discountAmountToPercent()
      this.inputDiscountEdit = true
    },
    eidtDiscount (multipleValue, addValue) {
      if (!this.inputDiscountEdit) {
        return
      }

      if (this.inputDiscountType === 1) {
        var newDsctPercent = Math.floor(
          this.inputDiscountPercent * multipleValue + addValue
        )
        if (newDsctPercent <= 100) {
          this.inputDiscountPercent = newDsctPercent
          // 割引対象（小計+サービス料）
          const disconutBase = this.paymentTotalInfo.subTotalAmount + this.paymentTotalInfo.serviceAmount
          this.inputDiscountAmount = Math.floor(
            (disconutBase * newDsctPercent) / 100
          )
        }
      } else {
        this.inputDiscountAmount = Math.floor(
          this.inputDiscountAmount * multipleValue + addValue
        )
        this.discountAmountToPercent()
      }
      // this.inputDiscountAmount = Math.floor(this.inputDiscountAmount * multipleValue + addValue)
    },
    discountAmountToPercent () {
      // 割引対象（小計+サービス料）
      const disconutBase = this.paymentTotalInfo.subTotalAmount + this.paymentTotalInfo.serviceAmount

      if (disconutBase > 0) {
        this.inputDiscountPercent = Math.round(
          (this.inputDiscountAmount / disconutBase) *
            100
        )
      } else {
        this.inputDiscountPercent = 0
      }
    },
    endEditDiscount () {
      this.paymentTotalInfo.discountType = this.inputDiscountType
      this.paymentTotalInfo.discountPercent = this.inputDiscountPercent
      this.paymentTotalInfo.discountAmount = this.inputDiscountAmount
      this.inputDiscountEdit = false
    },
    cancelEditDiscount () {
      this.inputDiscountAmount = 0
      this.inputDiscountEdit = false
    },
    paymentClassKbnCss (pyClsKbn) {
      return this.paymentTotalInfo.paymentClassKbn === pyClsKbn
        ? 'green'
        : 'grey'
    },
    paymentKbnCss (pyKbn) {
      return this.paymentTotalInfo.paymentKbn === pyKbn ? 'green' : 'grey'
    },
    /**
     * レシート印刷
     */
    async printCashierChit () {
      if (this.paymentTotalInfo.subTotalQuantity <= 0) {
        this.$toasted.show('レシート対象注文を選択してください。', {
          theme: 'bubble',
          position: 'top-center',
          duration: 5000
        })
        return
      }

      this.printReceipt1 = true
      this.printReceipt2 = false
      if (this.cmptTotalAmount <= 0) {
        this.methodsToRunWhenContinue = this.doPrint
        this.continueConfirmDialog = true
      } else {
        await this.doPrint()
      }
    },
    /**
     * 領収書印刷
     */
    async printCashierReceipt () {
      if (this.paymentTotalInfo.subTotalQuantity <= 0) {
        this.$toasted.show('領収書対象注文を選択してください。', {
          theme: 'bubble',
          position: 'top-center',
          duration: 5000
        })
        return
      }

      this.printReceipt1 = false
      this.printReceipt2 = true
      if (this.cmptTotalAmount <= 0) {
        this.methodsToRunWhenContinue = this.doPrint
        this.continueConfirmDialog = true
        // 2021/07/06 レシートが必要ない場合追加
      } else {
        await this.doPrint()
      }
    },
    // 2021/07/06 レシートが必要ない場合追加
    /**
     * withReceipt レシート
     * withReceiptChit 領収書
     */
    async doPrint () {
      this.continueConfirmDialog = false
      const postData = {
        storeNumber: SysConst.storeNumber,
        storeInfo: this.storeInfo,
        seatTakeoutRecpInfo: this.seatTakeoutRecpInfo,
        orderInfoList: this.orderInfoList,
        paymentClassKbn: this.paymentTotalInfo.paymentClassKbn,
        paymentKbn: this.paymentTotalInfo.paymentKbn,
        discountType: this.paymentTotalInfo.discountType,
        discountPercent: this.paymentTotalInfo.discountPercent,
        discountAmount: this.paymentTotalInfo.discountAmount,
        serviceAmount: this.paymentTotalInfo.serviceAmount,
        // レシート
        withReceipt: this.printReceipt1,
        // 領収書
        withReceiptChit: this.printReceipt2
      }
      var printSuccessAll = true

      if (this.printReceipt1 || this.printReceipt2) {
        const printDataResponse = await this.$lwHttp.postAsync(
          'print/getOrdersCashierChitPrintData',
          postData
        )

        var printerDataList = printDataResponse.printerDataList

        for (var pntIdx = 0; pntIdx < printerDataList.length; pntIdx++) {
          var printerData = printerDataList[pntIdx]
          var printXmlDatas = printerData.printXmlDatas

          for (let xmlIdx = 0; xmlIdx < printXmlDatas.length; xmlIdx++) {
            const xmlStr = printXmlDatas[xmlIdx]
            var printRslt = await this.$lwHttp.printChit(
              printerData.printerUri,
              xmlStr
            )
            var printSuccess = printRslt.success

            printSuccessAll = printSuccessAll && printSuccess === 'true'
            if (printSuccess === 'false') {
              var printerName = printerData.printerName
              var msgCode = printRslt.code
              var msgStr = printRslt.message
              this.$toasted.error(
                `[プリンター${printerName}] 印刷エラー [${msgCode}]：${msgStr}`,
                {
                  theme: 'bubble',
                  position: 'top-center',
                  duration: 8000,
                  icon: 'print_disabled'
                }
              )
            }
          }
        }
      }
    },
    async paymentBtnClicked () {
      if (this.orderInfoList.some(od => od.casherPaySelected === 1 && od.paymentOver === 1)) {
        this.$toasted.show('会計済みの注文を外してください。', {
          theme: 'bubble',
          position: 'top-center',
          duration: 5000
        })
        return
      }

      if (this.cmptTotalAmount <= 0) {
        this.methodsToRunWhenContinue = this.doPayment
        this.continueConfirmDialog = true
      } else {
        await this.doPayment()
      }
    },
    async doPayment () {
      this.continueConfirmDialog = false
      await this.printCashierChit()

      const postData = {
        storeNumber: SysConst.storeNumber,
        storeInfo: this.storeInfo,
        seatTakeoutRecpInfo: this.seatTakeoutRecpInfo,
        orderInfoList: this.orderInfoList,
        paymentClassKbn: this.paymentTotalInfo.paymentClassKbn,
        paymentKbn: this.paymentTotalInfo.paymentKbn,
        discountType: this.paymentTotalInfo.discountType,
        discountPercent: this.paymentTotalInfo.discountPercent,
        discountAmount: this.paymentTotalInfo.discountAmount,
        serviceAmount: this.paymentTotalInfo.serviceAmount,
        withReceipt: false,
        withReceiptChit: false
      }

      const orderPaymentRsp = await this.$lwHttp.postAsync(
        'cashier/orderPayment',
        postData
      )
      this.storeInfo = orderPaymentRsp.storeInfo
      this.seatTakeoutRecpInfo = orderPaymentRsp.seatTakeoutRecpInfo
      this.orderInfoList.splice(0)
      this.orderInfoList.push(...orderPaymentRsp.orderInfoList)

      this.paymentGpClassLst.splice(0)
      this.paymentGpClassLst.push(...orderPaymentRsp.paymentGpClassLst)

      this.paymentClassLstAll.splice(0)
      this.paymentClassLstAll.push(...orderPaymentRsp.paymentClassLst)

      this.paymentTotalInfo = {
        subTotalQuantity: 0,
        subTotalAmount: 0,
        discountType: 1,
        discountPercent: 0,
        discountAmount: 0,
        serviceAmount: 0,
        totalTaxAmount: 0,
        paymentClassKbn: '',
        paymentKbn: ''
      }
      this.changePaymentClassKbn(
        this.paymentGpClassLst[0].categoryClassNumber
      )
    },
    async doPrintTest (withReceiptChit) {
      const postData = {
        storeNumber: SysConst.storeNumber,
        storeInfo: this.storeInfo,
        seatTakeoutRecpInfo: this.seatTakeoutRecpInfo,
        orderInfoList: this.orderInfoList,
        paymentClassKbn: this.paymentTotalInfo.paymentClassKbn,
        paymentKbn: this.paymentTotalInfo.paymentKbn,
        discountType: this.paymentTotalInfo.discountType,
        discountPercent: this.paymentTotalInfo.discountPercent,
        discountAmount: this.paymentTotalInfo.discountAmount,
        serviceAmount: this.paymentTotalInfo.serviceAmount,
        withReceipt: this.printReceipt1,
        withReceiptChit: this.printReceipt2
      }
      var printSuccessAll = true

      if (withReceiptChit) {
        const printDataResponse = await this.$lwHttp.postAsync(
          'print/getOrdersCashierChitPrintData',
          postData
        )
        this.chitXmlText = ''
        var printerDataList = printDataResponse.printerDataList

        for (var pntIdx = 0; pntIdx < printerDataList.length; pntIdx++) {
          var printerData = printerDataList[pntIdx]
          var printXmlDatas = printerData.printXmlDatas

          for (let xmlIdx = 0; xmlIdx < printXmlDatas.length; xmlIdx++) {
            const xmlStr = printXmlDatas[xmlIdx]
            this.chitXmlText = this.chitXmlText + xmlStr
          }
        }
        this.chitXmlDialog = true
      }
      if (printSuccessAll) {
        const orderPaymentRsp = await this.$lwHttp.postAsync(
          'cashier/orderPayment',
          postData
        )
        this.storeInfo = orderPaymentRsp.storeInfo
        this.seatTakeoutRecpInfo = orderPaymentRsp.seatTakeoutRecpInfo
        this.orderInfoList.splice(0)
        this.orderInfoList.push(...orderPaymentRsp.orderInfoList)

        this.paymentGpClassLst.splice(0)
        this.paymentGpClassLst.push(...orderPaymentRsp.paymentGpClassLst)

        this.paymentClassLstAll.splice(0)
        this.paymentClassLstAll.push(...orderPaymentRsp.paymentClassLst)

        this.paymentTotalInfo = {
          subTotalQuantity: 0,
          subTotalAmount: 0,
          discountType: 1,
          discountPercent: 0,
          discountAmount: 0,
          serviceAmount: 0,
          totalTaxAmount: 0,
          paymentClassKbn: '',
          paymentKbn: ''
        }
        this.changePaymentClassKbn(
          this.paymentGpClassLst[0].categoryClassNumber
        )
      }
    },
    copyXmlToClipboard () {
      var _this = this
      navigator.clipboard.writeText(this.chitXmlText).then(function () {
        _this.$toasted.error(
          '帳票内容をコピーしました',
          {
            theme: 'bubble',
            position: 'top-center',
            duration: 8000,
            icon: 'print_disabled'
          }
        )
      }, function () {
        _this.$toasted.error(
          '帳票内容をコピー失敗',
          {
            theme: 'bubble',
            position: 'top-center',
            duration: 8000,
            icon: 'print_disabled'
          }
        )
      })
    }
  }
}
</script>
<style scoped>
.green-label /deep/ label {
  color: rgb(67, 160, 71);
}
.blue-label /deep/ label {
  color: rgb(13, 71, 161);
}
</style>
