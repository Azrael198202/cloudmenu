<template>
  <div style="min-width:1600px;user-select:none;">
    <!-- ***************************************************************************************************************** -->
    <v-container fluid class="grey lighten-4 elevation-1" style="height:80px;position:sticky;top:0px">
      <v-row no-gutters justify="space-between">
          <v-col cols="6" class="pt-3 pr-4">
            <div class="pl-4">
              <div class="title font-weight-bold">{{SysConst.storeName}}</div>
              <!-- <div class="title font-weight-bold">座席状況一覧</div> -->
            </div>
          </v-col>
          <v-col cols="6" class="text-right pt-3 pr-4">
            <span class="subtitle-1">{{nowStrJp}}</span>
          </v-col>
      </v-row>
    </v-container>
    <!-- ***************************************************************************************************************** -->
    <v-container fluid class="grey lighten-3" style="height:850px">
      <v-row>
        <v-col class="text-right">
          <router-link to='/home'>
            <v-btn width="180px" height="180px" style="background: #4685ff; color: #fff" class="flex-btn">
              <div><img src="../assets/images/menu.png" alt="" width="80px" class="flex-btn-icon"></div>
              <div class="flex-btn-text">注文状況一覧</div>
            </v-btn>
          </router-link>
        </v-col>
        <v-col class="text-left">
          <router-link to='/moneyinout'>
            <v-btn width="180px" height="180px" style="background: #ff9e35; color: #fff" class="flex-btn"
            >
              <div><img src="../assets/images/money.png" alt="" width="70px" class="flex-btn-icon2"></div>
              <div class="flex-btn-text2">入出金</div>
            </v-btn>
          </router-link>
        </v-col>
      </v-row>
      <v-row>
        <v-col class="text-right">
          <v-btn width="180px" height="180px" class="flex-btn teal white--text"
            @click="openDailyReport()"
          >
              <div><img src="../assets/images/receipt.png" alt="" width="70px" class="flex-btn-icon2"></div>
            <div class="flex-btn-text2" style="left:20px">日計レポート</div>
          </v-btn>
        </v-col>
        <v-col class="text-left">
          <v-btn width="180px" height="180px" class="flex-btn teal white--text"
            @click="openPaymentHistory()"
          >
            <v-icon size="80" class="flex-btn-icon2">mdi-cash-register</v-icon>
            <div class="flex-btn-text2" style="left:40px">会計履歴</div>
          </v-btn>
        </v-col>
      </v-row>
      <v-row>
        <v-col class="text-right" cols="6">
          <v-btn width="180px" height="180px" style="background: #FF3300; color: #fff"  class="flex-btn"
            @click="openDailyReport()"
          >
              <div><img src="../assets/images/receipt.png" alt="" width="70px" class="flex-btn-icon2"></div>
            <div class="flex-btn-text2" style="left:20px">取消レポート</div>
          </v-btn>
          <v-col class="text-left"></v-col>
        </v-col>
      </v-row>
    </v-container>
    <!-- ***************************************************************************************************************** -->
    <v-dialog persistent width="700px"
      v-model="dailyReportFlg"
    >
      <daily-report ref="dailyReport"
        @dailyReportClose="dailyReportFlg=false"
      >
      </daily-report>
    </v-dialog>
    <!-- ***会計履歴***** -->
    <v-dialog persistent width="600px"
      v-model="paymentHisFlg"
    >
      <payment-history ref="paymentHistory"
        @paymentHistoryClose="paymentHisFlg=false"
      >
      </payment-history>
    </v-dialog>
  </div>
</template>
<script>
import SysConst from '../lwUtils/SysConst'
import moment from 'moment'
import DailyReport from '../components/DailyReport'
import PaymentHistory from '../components/PaymentHistory.vue'
export default {
  name: 'Menu',
  components: {
    DailyReport,
    PaymentHistory
  },
  data: () => ({
    SysConst: SysConst,
    msgIndex: 0,
    nowStrJp: '',
    btnInfo: [
      { text: '商品登録', color: '#4685ff', x_position: 1, y_position: 1 },
      { text: '店舗情報登録', color: '#4685ff', x_position: 2, y_position: 1 },
      { text: '座席登録', color: '#4685ff', x_position: 3, y_position: 1 },
      { text: '区分マスタ登録', color: '#4685ff', x_position: 4, y_position: 1 },
      { text: '入出金', color: '#4685ff', x_position: 5, y_position: 1 },
      { text: 'メニュー登録', color: '#ff9e35', x_position: 1, y_position: 2 },
      { text: '在庫一覧', color: '#54c336', x_position: 1, y_position: 3 },
      { text: '入庫一覧', color: '#54c336', x_position: 2, y_position: 3 }
    ],
    y_arr: [1, 2, 3],
    /* ************************************************************* */
    dailyReportFlg: false,
    /* ************************************************************* */
    paymentHisFlg: false
  }),
  mounted () {
    moment.locale('ja')
    this.nowStrJp = moment().format('llll')

    // setInterval(() => {
    //   this.msgIndex = (this.msgIndex + 1) % 3
    //   this.nowStrJp = moment().format('llll')
    // }, 3500)
  },
  methods: {
    openDailyReport () {
      this.dailyReportFlg = true
      this.$nextTick(
        () => {
          this.$refs.dailyReport.initForm()
        }
      )
    },
    openCancelReport () {
      this.dailyReportFlg = true
      this.$nextTick(
        () => {
          this.$refs.dailyReport.initForm()
        }
      )
    },
    openPaymentHistory () {
      this.paymentHisFlg = true
      this.$nextTick(
        () => {
          this.$refs.paymentHistory.initForm()
        }
      )
      // this.$router.push({ path: '/cashier/20210713004' })
    }
  }
}
</script>

<style scoped>

.flex-btn {
  position: relative;
}

.flex-btn-icon {
  position: absolute;
  left: 36px;
  bottom: -6px;
}

.flex-btn-text {
  font-size: 18px;
  font-weight: bold;
  position: absolute;
  left: 18px;
  bottom: -50px;
}

.flex-btn-icon2 {
  position: absolute;
  left: 40px;
  bottom: -10px;
}

.flex-btn-text2 {
  font-size: 18px;
  font-weight: bold;
  position: absolute;
  bottom: -50px;
}

</style>
