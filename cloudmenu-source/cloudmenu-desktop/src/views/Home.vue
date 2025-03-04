<template>
  <div class="" style="width:1903px;border: 2px solid #ddd;">
    <!-- ***Header************************************************************************************************************** -->
    <v-container fluid class="white" style="height:75px;position:sticky;top:0px;">
      <v-row no-gutters align="center">
        <v-col cols="3">
          <v-row no-gutters align="center">
            <v-icon large class="mr-2" color="red"
              @click="routeBack()"
            >mdi-backburger</v-icon>
            <div style="display:inline-block">
              <div class="subtitle-1 font-weight-bold">{{SysConst.storeName}}</div>
              <div class="title font-weight-bold">座席状況一覧</div>
            </div>
          </v-row>
        </v-col>
        <v-col cols="1" class="text-right">
          <v-badge v-if="notifyCount>0" overlap color="red" class="mr-5" :content="notifyCount">
            <v-icon @click="notifyDialogFlg=true" large>mdi-bell-outline</v-icon>
          </v-badge>
          <v-icon v-else large>mdi-bell-outline</v-icon>
        </v-col>
        <v-col cols="5">
          <span class="subtitle-1 font-weight-medium">
              <span class="red--text pr-1">{{notifyToShow.seatTakeoutName}}</span>{{notifyToShow.notifyMsg}}
          </span>
        </v-col>
        <v-col cols="3" class="text-right pt-3 pr-4">
          <span class="subtitle-1">{{nowStrJp}}</span>
        </v-col>
      </v-row>
    </v-container>
    <!-- ***Body************************************************************************************************************** -->
    <v-container fluid class="grey lighten-3" style="height:860px">
      <v-row no-gutters style="height:100%">
        <!-- ***Left Panel********************************************************************************************** -->
        <div style="width:630px;height:100%" class="pa-2">
          <!-- ***Room Summary************************************************************************************* -->
          <v-row style="height:135px" class="">
            <div class="ma-2" style="width:35%"
              @click="showSeatRecpDialog"
            >
              <room-sum :sumName="hallName"
                :sumCount="hallCount"
                :sumEmptyCount="hallEmptyCount"
                :sumUsingCount="hallUsingCount"
                :sumNotifyCount="hallNotifyCount"
                >
              </room-sum>
            </div>
            <div  class="ma-2" style="width:35%"
              @click="showSeatRecpDialog"
            >
              <room-sum :sumName="roomName"
                :sumCount="roomCount"
                :sumEmptyCount="roomEmptyCount"
                :sumUsingCount="roomUsingCount"
                :sumNotifyCount="roomNotifyCount"
                @click="showTakeoutDialog"
                >
              </room-sum>
            </div>
            <!-- 2021/07/01 持ち帰りクリック方法変更 -->
            <div class="ma-2" style="width:22%;cursor: pointer;"
              @click="showTakeoutDialog"
            >
              <take-away :takeOutName="takeOutName"
                :takeOutCount="takeOutSumInfoList.length">
              </take-away>
            </div>
          </v-row>
          <!-- ***Floor Plan************************************************************************************* -->
          <div class="floor-Paln-div mt-3 white">
            <room-info v-for="(seatSumInfo, index) in seatSumInfoList" :key="index" :seatSumInfo="seatSumInfo"
              @seatInfoClicked="toShowRecpOders"
              @cashierMenuClick="gotoCashier"
            ></room-info>
          </div>
        </div>
        <!-- ***Right Panel********************************************************************************************** -->
        <div style="width:1235px;height:100%" class="ml-2">
          <!-- ***Room Customer Infos********************************************************************************* -->
          <table style="width:100%;height:100px" class="font-weight-bold grey lighten-5">
            <tr >
              <td width="100px">
                <div class="pl-4 pt-5" style="height:100%">
                  {{selectedSeatTakeoutRecpInfo.receptionKbnName}}
                </div>
              </td>
              <td width="500px" class="">
                <table style="width:100%;height:100%" class="">
                  <tr>
                    <td width="20%">{{selectedSeatTakeoutRecpInfo.receptionNumber}}</td>
                    <td width="20%">{{selectedSeatTakeoutRecpInfo.seatMergeAll}}</td>
                    <td width="30%">{{selectedSeatTakeoutRecpInfo.customRepresentName}}</td>
                    <td width="30%"><span class="grey--text">着席時間: </span>{{selectedSeatTakeoutRecpInfo.timePassedSinceStart}}</td>
                  </tr>
                  <tr style="vertical-align:top;">
                    <td><span class="grey--text">男: </span>{{selectedSeatTakeoutRecpInfo.seatPeopleMan}}名</td>
                    <td><span class="grey--text">女: </span>{{selectedSeatTakeoutRecpInfo.seatPeopleWoman}}名</td>
                    <td><span class="grey--text">子供: </span>{{selectedSeatTakeoutRecpInfo.seatPeopleChildren}}名</td>
                    <td><span class="grey--text">計: </span>{{selectedSeatTakeoutRecpInfo.seatPelpleAll}}名</td>
                  </tr>
                </table>
              </td>
              <td width="50px" style="vertical-align:top;" class="text-right grey--text pt-3">
                  備考：
              </td>
              <td class="pt-3 pr-2">
                <textarea rows="4" cols="67" class="rounded" style="border:solid 1px gray;resize: none;"
                  readonly v-model="selectedSeatTakeoutRecpInfo.receptionRemarks"
                  @click="openReceiptRemarkDialog(selectedSeatTakeoutRecpInfo)"
                >
                </textarea>
              </td>
            </tr>
          </table>
          <!-- ***Food List********************************************************************************* -->
          <v-data-table
            disable-pagination
            fixed-header
            hide-default-footer
            :headers="foodLstHeaders"
            :items="selectingOrderList"
            :no-data-text="'注文なし'"
            class="elevation-1 foodList"
            height="688px"
          >
            <!-- @click:row="toggleSelectRow" -->

            <!-- ヘッダ-料理区分 -->
            <template v-slot:header.productTypeName>
                <span>料理区分</span><v-icon small>mdi-sort</v-icon>
            </template>
            <!-- ヘッダ-経過 -->
            <template v-slot:header.timePassedSinceOrder>
                <span>経過</span><v-icon small>mdi-sort</v-icon>
            </template>
            <!-- データ-No -->
            <template v-slot:item.orderVoucherDetails="{ item }">
              <div @click="toggleSelectRow(item)" style="user-select:none;">
                {{item.orderVoucherDetails}}
              </div>
            </template>
            <!-- データ-料理名 -->
            <template v-slot:item.productShowName="{ item }">
              <div>
                <span>{{item.productShowName}}</span><span class="pl-2 caption orange--text">{{item.takeoutName}}</span>
                <div class="text-truncate caption grey--text" style="max-width: 185px;">{{item.productShowNameJp}}</div>
                <div class="text-truncate caption blue--text" style="max-width: 185px;">{{item.productSetName}}</div>
              </div>
            </template>
            <!-- データ - 数量 -->
            <template v-slot:item.orderQuantity="{ item }">
                <!-- <span class="ml-r" >×</span> -->
                <div class="rounded-sm editableBorder"
                  @click="openNumInpuDialog(item,'orderQuantity')"
                >{{item.orderQuantity}}</div>
            </template>
             <!-- データ - 金額 -->
            <template v-slot:item.orderDiscountYen="{ item }">
                <!-- <span class="ml-r" >×</span> -->
                <div class="rounded-sm editableBorder"
                  @click="openNumInpuDialog(item,'orderDiscountYen')"
                >{{jpCurrencyFmt.format(item.orderDiscountYen)}}</div>
            </template>
            <!-- データ - 総額 -->
            <template v-slot:item.orderAfterPrice="{ item }">
                <!-- <span class="mr-2">=</span> -->
                {{jpCurrencyFmt.format(item.orderQuantity * item.orderDiscountYen)}}
            </template>
            <!-- データ - 備考 -->
            <template v-slot:item.orderRemarks="{ item }">
              <div class="rounded-sm editableBorder" style="min-height:30px"
                  @click="openOrderRemarkDialog(item)"
                >{{item.orderRemarks}}
              </div>
            </template>
            <!-- データ - 調理 -->
            <template v-slot:item.orderCookingState="{ item }">
              <v-avatar class="rounded white--text" size="36"
                v-bind:class="{ grey:item.orderCookingFlg === '1',orange:item.orderCookingFlg==='0'}"
              >{{item.orderCookingState}}</v-avatar>
            </template>
            <!-- データ - 配膳 -->
            <template v-slot:item.orderServeState="{ item }">
              <v-avatar tile class="rounded white--text" size="36"
                v-bind:class="{ grey:item.orderServeFlg === '1',orange:item.orderServeFlg==='0'}"
                @click="toggleServeFlg(item)"
              >{{item.orderServeState}}</v-avatar>
            </template>
          </v-data-table>
          <!-- ***Food Footer********************************************************************************* -->
          <div class="text-center white rounded" style="width:100%;height:30px;">
              <span>合計：</span><span>{{selectedSeatTakeoutRecpInfo.orderDetailCount}}</span><span>点</span>
              <span class="red--text ml-4 title">{{jpCurrencyFmt.format(selectedSeatTakeoutRecpInfo.orderPriceSum)}}</span>
          </div>
        </div>
      </v-row>
    </v-container>
    <!-- ***Footer************************************************************************************************************** -->
    <v-container fluid class="white" style="height:50px">
      <v-row >
        <v-col class="pt-5 font-weight-bold">Copyright © 2021 LeadingWin Corporation, All Rights Reserved.</v-col>
        <!-- <v-col class="pt-2 pr-8 text-right">
          <v-btn class="red white--text"
            :disabled = "!selectedOrders.length"
            @click="printOrdersCookChit"
          >
              伝票出力
          </v-btn>
        </v-col> -->
      </v-row>
    </v-container>
    <!-- ***持ち帰り詳細Dialogs************************************************************************************************************ -->
    <v-dialog scrollable persistent width="800px"
      v-model="takeoutDialogFlg">
      <takeout-receiption-list
        :takeOutSumInfoList="takeOutSumInfoList"
        @closeBtnClicked="takeoutDialogFlg=false"
        @recpDetailClicked="toShowRecpOders"
        @recpCashierClicked="gotoCashier"
        @deleteClicked="deleteReceiption"
      >
      </takeout-receiption-list>
    </v-dialog>
    <!-- ***店内受付詳細Dialogs************************************************************************************************************ -->
    <v-dialog scrollable persistent width="800px"
      v-model="seatRecpDialogFlg">
      <seat-receiption-list
        :seatSumInfoList="seatSumInfoList"
        @closeBtnClicked="seatRecpDialogFlg=false"
        @recpCashierClicked="gotoCashier"
        @deleteClicked="deleteReceiption"
      >
      </seat-receiption-list>
    </v-dialog>
    <!-- ***通知詳細Dialogs************************************************************************************************************ -->
    <v-dialog scrollable persistent width="500px"
      v-model="notifyDialogFlg">
      <v-card>
        <v-card-title>
          <v-row :align="'center'">
            <v-col cols="6">警告</v-col>
            <v-col cols="6" class="text-right">
              <v-btn icon color="red"
                @click="notifyDialogFlg=false">
                <v-icon large>mdi-close-circle</v-icon>
              </v-btn>
            </v-col>
          </v-row>
        </v-card-title>
        <v-divider></v-divider>
        <v-card-text style="height:500px;">
          <v-container>
            <v-row v-for="(notify,index) in notifyList" :key="index">
              <span class="subtitle-1 font-weight-medium">
                <span class="red--text pr-1">{{notify.seatTakeoutName}}</span>{{notify.notifyMsg}}
              </span>
            </v-row>
          </v-container>
        </v-card-text>
        <v-divider></v-divider>
      </v-card>
    </v-dialog>
    <!-- ***電卓Dialogs************************************************************************************************************ -->
    <v-dialog scrollable width="280px"
      v-model="numInputerDialog.flg">
      <v-card class="pa-3">
        <v-container>
          <v-row>
            <v-col cols="12" class="text-right" align-self="center">
                <div class="rounded title elevation-2 pa-2">{{numInputerDialog.numValue}}</div>
                <div class="text-caption text-left red--text"><pre>{{numInputerDialog.errMsg}}</pre></div>
            </v-col>
          </v-row>
          <v-row align="center">
            <v-col class="text-center" cols="4">
                <div class="rounded headline elevation-2 pa-3 inputerNum"
                  @click="numInputed(9)"
                >9</div>
            </v-col>
            <v-col class="text-center" cols="4">
                <div class="rounded headline elevation-2 pa-3 inputerNum"
                  @click="numInputed(8)"
                >8</div>
            </v-col>
            <v-col class="text-center" cols="4">
                <div class="rounded headline elevation-2 pa-3 inputerNum"
                  @click="numInputed(7)"
                >7</div>
            </v-col>
          </v-row>
          <v-row align="center">
            <v-col class="text-center" cols="4">
                <div class="rounded headline elevation-2 pa-3 inputerNum"
                  @click="numInputed(6)"
                >6</div>
            </v-col>
            <v-col class="text-center" cols="4">
                <div class="rounded headline elevation-2 pa-3 inputerNum"
                  @click="numInputed(5)"
                >5</div>
            </v-col>
            <v-col class="text-center" cols="4">
                <div class="rounded headline elevation-2 pa-3 inputerNum"
                  @click="numInputed(4)"
                >4</div>
            </v-col>
          </v-row>
          <v-row align="center">
            <v-col class="text-center" cols="4">
                <div class="rounded headline elevation-2 pa-3 inputerNum"
                  @click="numInputed(3)"
                >3</div>
            </v-col>
            <v-col class="text-center" cols="4">
                <div class="rounded headline elevation-2 pa-3 inputerNum"
                  @click="numInputed(2)"
                >2</div>
            </v-col>
            <v-col class="text-center" cols="4">
                <div class="rounded headline elevation-2 pa-3 inputerNum"
                  @click="numInputed(1)"
                >1</div>
            </v-col>
          </v-row>
          <v-row align="center">
            <v-col class="text-center" cols="4">
                <div class="rounded headline elevation-2 pa-3 inputerNum"
                  @click="numInputed(0)"
                >0</div>
            </v-col>
            <v-col class="text-center" cols="4">
                <div class="rounded elevation-2 pt-4 inputerNum"
                  @click="numDel()"
                >削除</div>
            </v-col>
            <v-col class="text-center" cols="4" :align-self="'start'">
                <div class="rounded elevation-2 pt-4 inputerNum green darken-3 white--text"
                  @click="updateOrder()"
                >決定</div>
            </v-col>
          </v-row>
        </v-container>
      </v-card>
    </v-dialog>
    <!-- ***備考Dialogs************************************************************************************************************ -->
    <v-dialog scrollable width="600px"
      v-model="markInputerDialog.flg">
      <v-card class="pa-3">
        <v-container>
          <v-row>
            <v-col cols="12" class="text-right" align-self="center">
                <v-textarea outlined label="備考" auto-grow
                  v-model="markInputerDialog.remarkValue"
                ></v-textarea>
            </v-col>
          </v-row>
          <v-row align="center">
            <v-col class="text-center" cols="6">
                <div class="rounded elevation-2 pt-4 inputerNum"
                  @click="closeOrderRemarkDialog()"
                >キャンセル</div>
            </v-col>
            <v-col class="text-center" cols="6" :align-self="'start'">
                <div class="rounded elevation-2 pt-4 inputerNum green darken-3 white--text"
                  @click="updateRemark()"
                >決定</div>
            </v-col>
          </v-row>
        </v-container>
      </v-card>
    </v-dialog>
  </div>
</template>
<script>
import moment from 'moment'
import RoomSum from '../components/RoomSum'
import TakeAway from '../components/TakeAway'
import RoomInfo from '../components/RoomInfo'
import TakeoutReceiptionList from '../components/TakeoutReceiptionList'
import SeatReceiptionList from '../components/SeatReceiptionList'
import SysConst from '../lwUtils/SysConst'

// eslint-disable-next-line no-unused-vars
const jpCurrencyFmt = new Intl.NumberFormat('ja-JP', { style: 'currency', currency: 'JPY' })

export default {
  name: 'Home',
  components: {
    RoomSum,
    TakeAway,
    RoomInfo,
    TakeoutReceiptionList,
    SeatReceiptionList
  },
  data: () => ({
    SysConst: SysConst,
    // 間隔的更新処理のID
    refeshIntervalID: null,
    refeshIntervalTime: 3500,
    // 日本金額フォマード
    jpCurrencyFmt: jpCurrencyFmt,
    // システム日時 eg 2021/05/17(月) 12:30
    nowStrJp: '',
    // 通知情報リスト
    notifyList: [],
    // 通知件数
    notifyCount: 0,
    // 表示する通知データ
    notifyIndexToShow: 0,
    notifyToShow: {
      notifyKbn: 0,
      seatTakeoutName: '',
      notifyMsg: ''
    },
    // ホール情報
    hallName: 'ホール',
    hallCount: 0,
    hallEmptyCount: 0,
    hallUsingCount: 0,
    hallNotifyCount: 0,
    // 個室情報
    roomName: '個室',
    roomCount: 0,
    roomEmptyCount: 0,
    roomUsingCount: 0,
    roomNotifyCount: 0,
    // 持ち帰り情報
    takeOutName: '持ち帰り',
    takeOutSumInfoList: [],
    // 座席総合情報
    // { roomName: '個室1', psTop: 295, psLeft: 455, roomStatus: 3, roomCustomerCount: 3, roomConsumAmount: 11300 },
    // { roomName: '個室2', psTop: 155, psLeft: 455, roomStatus: 2, roomCustomerCount: 4, roomConsumAmount: 21300 },
    // { roomName: '個室3', psTop: 10, psLeft: 455, roomStatus: 1, roomCustomerCount: 6, roomConsumAmount: 5300 },
    // { roomName: '個室4', psTop: 20, psLeft: 100, roomStatus: 1, roomCustomerCount: 2, roomConsumAmount: 11300 },
    // { roomName: '個室5', psTop: 200, psLeft: 100, roomStatus: 4, roomCustomerCount: 1, roomConsumAmount: 41300 },
    // { roomName: 'ホール1', psTop: 390, psLeft: 15, roomStatus: 0, roomCustomerCount: 3, roomConsumAmount: 32300 },
    // { roomName: 'ホール2', psTop: 390, psLeft: 155, roomStatus: 1, roomCustomerCount: 2, roomConsumAmount: 1300 },
    // { roomName: 'ホール3', psTop: 570, psLeft: 15, roomStatus: 0, roomCustomerCount: 1, roomConsumAmount: 1300 },
    // { roomName: 'ホール4', psTop: 570, psLeft: 155, roomStatus: 3, roomCustomerCount: 7, roomConsumAmount: 129300 },
    // { roomName: 'ホール5', psTop: 570, psLeft: 295, roomStatus: 0, roomCustomerCount: 3, roomConsumAmount: 22300 },
    // { roomName: 'ホール6', psTop: 570, psLeft: 435, roomStatus: 0, roomCustomerCount: 1, roomConsumAmount: 45600 }
    seatSumInfoList: [],
    // 選択されたホール・個室・持ち帰り注文情報
    selectedSeatTakeoutRecpInfo: {},
    // 選択されている受付の注文一覧
    selectingOrderList: [],
    selectedOrders: [],
    // 選択されている受付番号
    selectedRecpNum: '',
    foodLstHeaders: [
      { width: '80px', text: 'No', value: 'orderVoucherDetails', divider: false, align: 'center', sortable: false, class: 'grey lighten-2 subtitle-1 font-weight-bold' },
      { width: '70px', text: '座席', value: 'seatName', divider: false, align: 'center', sortable: false, class: 'grey lighten-2 subtitle-1 font-weight-bold' },
      { width: '140px', text: '料理区分', value: 'productTypeName', divider: false, align: 'start', sortable: true, class: 'grey lighten-2 subtitle-1 font-weight-bold' },
      { width: '220px', text: '料理名', value: 'productShowName', divider: false, align: 'start', sortable: false, class: 'grey lighten-2 subtitle-1 font-weight-bold' },
      { width: '80px', text: '数量', value: 'orderQuantity', divider: false, align: 'end', sortable: false, class: 'grey lighten-2 subtitle-1 font-weight-bold' },
      { width: '100px', text: '金額', value: 'orderDiscountYen', divider: false, align: 'end', sortable: false, class: 'grey lighten-2 subtitle-1 font-weight-bold' },
      { width: '100px', text: '総額', value: 'orderAfterPrice', divider: false, align: 'end', sortable: false, class: 'grey lighten-2 subtitle-1 font-weight-bold' },
      { width: '190px', text: '備考', value: 'orderRemarks', divider: false, align: 'start', sortable: false, class: 'grey lighten-2 subtitle-1 font-weight-bold' },
      { width: '100px', text: '経過', value: 'timePassedSinceOrder', divider: false, align: 'center', sortable: true, class: 'grey lighten-2 subtitle-1 font-weight-bold' },
      { text: '伝票', value: 'orderCookingState', divider: false, align: 'center', sortable: false, class: 'grey lighten-2 subtitle-1 font-weight-bold' },
      { text: '配膳', value: 'orderServeState', divider: false, align: 'center', sortable: false, class: 'grey lighten-2 subtitle-1 font-weight-bold' }
    ],
    // ***Dialog**********************************************************************
    takeoutDialogFlg: false,
    seatRecpDialogFlg: false,
    notifyDialogFlg: false,
    numInputerDialog: {
      flg: false,
      storeNumber: '',
      receptionNumber: '',
      receptionBranchNumber: 1,
      orderVoucherNumber: '',
      orderVoucherDetails: '',
      orderQuantity: 0,
      orderDiscountYen: 0,
      itemName: '',
      orgValue: 0,
      numValue: 0,
      deleteConfirmFlg: false,
      errMsg: ''
    },
    markInputerDialog: {
      flg: false,
      // 受付番号
      receptionNumber: '',
      // 注文番号（受付備考の場合は空白）
      orderVoucherDetails: '',
      // 備考
      remarkValue: ''
    }
  }),

  async mounted () {
    this.startIntervalRefesh()
  },
  beforeRouteLeave (to, from, next) {
    if (this.refeshIntervalID) {
      for (var i = 1; i <= this.refeshIntervalID; i++) {
        clearInterval(i)
      }
    }
    next()
  },
  methods: {
    routeBack () {
      this.$router.go(-1)
    },
    clearAllInterval () {
      clearInterval(this.refeshIntervalID)
      // if (this.refeshIntervalID) {
      //   for (var i = 1; i <= this.refeshIntervalID; i++) {
      //     clearInterval(i)
      //   }
      // }
    },
    async startIntervalRefesh () {
      this.clearAllInterval()
      await this.doRefesh()

      this.refeshIntervalID = setInterval(this.doRefesh, this.refeshIntervalTime)
      console.log('startIntervalRefesh-setInterval', this.refeshIntervalID)
    },
    // 画面情報更新
    async doRefesh () {
      const response = await this.$lwHttp.postAsync('seat/selectZasekiAllData', { storeNumber: SysConst.storeNumber, SelectedRecpNumber: this.selectedRecpNum })
      console.log(moment().format('hh:mm:ss'), '座席状況一覧-', '画面情報更新-', this.refeshIntervalID, response)
      this.refreshResponse(response)
    },
    refreshResponse (response) {
      // 更新間隔
      var rspRefeshIntervalTime = response.refeshIntervalTime
      if (Number.isInteger(rspRefeshIntervalTime)) {
        if (rspRefeshIntervalTime >= 1000) {
          this.refeshIntervalTime = rspRefeshIntervalTime
        }
      }

      // 座席総合情報
      this.seatSumInfoList.splice(0)
      this.seatSumInfoList.push(...response.seatSumInfoList)
      // 持ち帰り受付情報
      this.takeOutSumInfoList.splice(0)
      this.takeOutSumInfoList.push(...response.takeOutSumInfoList)
      // ホール情報
      this.hallCount = response.hallCount
      this.hallEmptyCount = response.hallEmptyCount
      this.hallUsingCount = response.hallUsingCount
      this.hallNotifyCount = response.hallNotifyCount
      // 個室情報
      this.roomCount = response.roomCount
      this.roomEmptyCount = response.roomEmptyCount
      this.roomUsingCount = response.roomUsingCount
      this.roomNotifyCount = response.roomNotifyCount
      // 選択されている受付情報
      this.selectedSeatTakeoutRecpInfo = response.selectedSeatTakeoutRecpInfo
      // 選択されている受付の注文一覧
      this.selectingOrderList.splice(0)
      this.selectingOrderList.push(...response.selectingOrderList)
      // 通知一覧
      this.notifyList.splice(0)
      this.notifyList.push(...response.notifyList)

      // システム日付
      moment.locale('ja')
      this.nowStrJp = moment().format('llll')
      // 通知件数更新
      this.notifyCount = this.notifyList.length
      if (this.notifyCount > 0) {
        this.notifyIndexToShow = (this.notifyIndexToShow + 1) % this.notifyCount
        this.notifyToShow = this.notifyList[this.notifyIndexToShow]
      } else {
        this.notifyToShow = { notifyKbn: 0, seatTakeoutName: '', notifyMsg: '' }
      }
    },
    // // 座席クリックで最新情報更新
    // seatInfoClicked (receptionNumber) {
    //   this.selectedRecpNum = receptionNumber
    //   this.startIntervalRefesh()
    // },
    // 座席・持ち帰りクリックで最新情報更新
    async toShowRecpOders (receptionNumber) {
      this.takeoutDialogFlg = false

      if (this.selectedRecpNum !== receptionNumber) {
        this.selectedOrders.splice(0)
      }
      this.selectedRecpNum = receptionNumber
      await this.startIntervalRefesh()
    },
    // 会計レジ画面に遷移
    gotoCashier (receptionNumber) {
      this.clearAllInterval()
      this.$router.push({ path: `/cashier/${receptionNumber}` })
    },
    // 受付を削除
    async deleteReceiption (receptionNumber) {
      this.clearAllInterval()
      const postData =
      {
        storeNumber: SysConst.storeNumber,
        receptionNumber: this.selectedRecpNum,
        receptionNumberToDel: receptionNumber
      }
      const response = await this.$lwHttp.postAsync('seat/deleteReception', postData)
      this.refreshResponse(response)

      this.refeshIntervalID = setInterval(this.doRefesh, this.refeshIntervalTime)
      console.log('updateOrder-setInterval', this.refeshIntervalID)
      this.$nextTick(
        () => {
          this.takeoutDialogFlg = false
          this.seatRecpDialogFlg = false
        }
      )
    },
    // 持ち帰り一覧を表示
    showTakeoutDialog () {
      if (this.takeOutSumInfoList.length) {
        this.takeoutDialogFlg = true
      }
    },
    // 受付一覧を表示
    showSeatRecpDialog () {
      this.seatRecpDialogFlg = true
    },
    openNumInpuDialog (orderInfo, itemName) {
      this.numInputerDialog.storeNumber = orderInfo.storeNumber
      this.numInputerDialog.receptionNumber = orderInfo.receptionNumber
      this.numInputerDialog.receptionBranchNumber = orderInfo.receptionBranchNumber
      this.numInputerDialog.orderVoucherNumber = orderInfo.orderVoucherNumber
      this.numInputerDialog.orderVoucherDetails = orderInfo.orderVoucherDetails
      this.numInputerDialog.orderQuantity = orderInfo.orderQuantity
      this.numInputerDialog.orderDiscountYen = orderInfo.orderDiscountYen
      this.numInputerDialog.itemName = itemName

      this.numInputerDialog.orgValue = orderInfo[itemName]
      this.numInputerDialog.numValue = orderInfo[itemName]

      this.numInputerDialog.deleteConfirmFlg = false
      this.numInputerDialog.errMsg = ''
      this.numInputerDialog.flg = true
    },
    numInputed (inputNum) {
      this.numInputerDialog.numValue = this.numInputerDialog.numValue * 10 + inputNum
    },
    numDel () {
      this.numInputerDialog.numValue = Math.floor(this.numInputerDialog.numValue / 10)
    },
    openReceiptRemarkDialog (receiptInfo) {
      if (receiptInfo.receptionNumber) {
        this.markInputerDialog.receptionNumber = receiptInfo.receptionNumber
        this.markInputerDialog.orderVoucherDetails = ''
        this.markInputerDialog.remarkValue = receiptInfo.receptionRemarks

        this.markInputerDialog.flg = true
      }
    },

    openOrderRemarkDialog (orderInfo) {
      this.markInputerDialog.receptionNumber = orderInfo.receptionNumber
      this.markInputerDialog.orderVoucherDetails = orderInfo.orderVoucherDetails
      this.markInputerDialog.remarkValue = orderInfo.orderRemarks

      this.markInputerDialog.flg = true
    },
    closeOrderRemarkDialog () {
      this.markInputerDialog.flg = false
    },
    async updateRemark () {
      this.clearAllInterval()
      const postData =
        {
          storeNumber: SysConst.storeNumber,
          receptionNumber: this.markInputerDialog.receptionNumber,
          orderVoucherDetails: this.markInputerDialog.orderVoucherDetails,
          remark: this.markInputerDialog.remarkValue
        }
      this.markInputerDialog.flg = false

      const response = await this.$lwHttp.postAsync('seat/updateRemark', postData)
      this.refreshResponse(response)

      this.refeshIntervalID = setInterval(this.doRefesh, this.refeshIntervalTime)
      console.log('updateRemark-setInterval', this.refeshIntervalID)

      this.markInputerDialog.flg = false
    },

    /**
     * 注文データの数量、金額を更新
     */
    async updateOrder () {
      if (this.numInputerDialog.itemName === 'orderQuantity') {
        if (this.numInputerDialog.orgValue < this.numInputerDialog.numValue) {
          this.numInputerDialog.errMsg = '注文の数量を増えることができません、\r\n新規注文してください。'
          return
        }

        if (this.numInputerDialog.numValue === 0 && !this.numInputerDialog.deleteConfirmFlg) {
          this.numInputerDialog.deleteConfirmFlg = true
          this.numInputerDialog.errMsg = '注文削除してよろしいですか?\r\n決定ボタンをもう一度押下してください。'
          return
        }
      }

      this.clearAllInterval()

      const postData =
        {
          storeNumber: SysConst.storeNumber,
          receptionNumber: this.numInputerDialog.receptionNumber,
          receptionBranchNumber: this.numInputerDialog.receptionBranchNumber,
          orderVoucherNumber: this.numInputerDialog.orderVoucherNumber,
          orderVoucherDetails: this.numInputerDialog.orderVoucherDetails,
          orderQuantity: this.numInputerDialog.orderQuantity,
          orderDiscountYen: this.numInputerDialog.orderDiscountYen
        }
      postData[this.numInputerDialog.itemName] = this.numInputerDialog.numValue

      const response = await this.$lwHttp.postAsync('seat/updateOrder', postData)
      this.refreshResponse(response)

      this.refeshIntervalID = setInterval(this.doRefesh, this.refeshIntervalTime)
      console.log('updateOrder-setInterval', this.refeshIntervalID)
      this.$nextTick(
        () => {
          this.numInputerDialog.flg = false
        }
      )
    },
    async toggleServeFlg (orderInfo) {
      this.clearAllInterval()

      const postData =
      {
        storeNumber: SysConst.storeNumber,
        receptionNumber: orderInfo.receptionNumber,
        receptionBranchNumber: orderInfo.receptionBranchNumber,
        orderVoucherNumber: orderInfo.orderVoucherNumber,
        orderVoucherDetails: orderInfo.orderVoucherDetails
      }
      const response = await this.$lwHttp.postAsync('seat/toggleServeFlg', postData)
      this.refreshResponse(response)

      this.refeshIntervalID = setInterval(this.doRefesh, this.refeshIntervalTime)
      console.log('toggleServeFlg-setInterval', this.refeshIntervalID)
      this.$nextTick(
        () => {
          this.numInputerDialog.flg = false
        }
      )
    },
    orderListItemClass (item) {
    //  return item.protein > 4.2 ? 'style-1' : 'style-2'
      return this.selectedOrders.includes(item.orderVoucherDetails) ? 'selectBg' : ''
    },
    toggleSelectRow (item) {
      const index = this.selectedOrders.indexOf(item.orderVoucherDetails)
      if (index >= 0) {
        this.selectedOrders.splice(index, 1)
      } else {
        this.selectedOrders.push(item.orderVoucherDetails)
      }
    }
  }
}
</script>
<style scoped>
  .floor-Paln-div{
    position: relative;
    height:705px;
    background-image:url('/img/FloorPlan.png');
    background-position:center;
  }
  .foodList{
    border: 1px solid #ddd;
  }
  .editableBorder{
    border: solid 1px gray;
    padding: 3px;
  }
  .inputerNum{
    margin: auto;
    user-select:none;
    height: 58px;
  }
</style>
