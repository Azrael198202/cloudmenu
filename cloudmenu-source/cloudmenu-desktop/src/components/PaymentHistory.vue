<template>
  <div>
    <v-container fluid class="white">
      <v-row align="center" class=" grey darken-3 white--text pr-2">
        <v-col class="pl-6 text-h6">
          会計履歴
        </v-col>
        <v-spacer></v-spacer>
        <v-btn icon color="red"
          @click="closeBtnClick()"
        >
          <v-icon large>mdi-close-circle</v-icon>
        </v-btn>
      </v-row>
    </v-container>
    <v-container fluid class="white">
      <v-menu
          v-model="receiptDateFlg"
          :close-on-content-click="false"
          :nudge-right="40"
          transition="scale-transition"
          offset-y
          min-width="auto"
        >
          <template v-slot:activator="{ on, attrs }">
            <v-text-field
              prepend-icon="mdi-calendar"
              label="レポート日付"
              v-model="receiptDate"
              readonly
              v-bind="attrs"
              v-on="on"
            ></v-text-field>
          </template>
          <v-date-picker
            v-model="receiptDate"
            @input="receiptDateFlg = false"
            locale="ja"
          ></v-date-picker>
      </v-menu>
    </v-container>
    <div class="grey lighten-4" style="height:650px;overflow: auto;">
      <div v-if="receiptionHisList.length===0" class="text-center text-h5">会計なし</div>
      <!-- 0:未注文 1:未会計 2:一部会計 3:会計済み -->
      <v-expansion-panels>
        <v-expansion-panel
          v-for="(rcpt,i) in receiptionHisList"
          :key="i"
        >
          <v-expansion-panel-header class="grey"
            v-bind:class="{'lighten-4': rcpt.paymentStatus===1 || rcpt.paymentStatus===2}">
            <v-row class="mr-4" align="center">
              <div style="width:180px" class="">
                受付：{{rcpt.receptionNumber}}
              </div>
              <div style="width:120px">
                {{rcpt.seatNames}}
              </div>
              <div style="width:80px">
                人数：{{rcpt.seatPeopleAll}}
              </div>
              <div class="">
                {{rcpt.paymentStatusName}}
              </div>
              <v-spacer></v-spacer>
            </v-row>
          </v-expansion-panel-header>
          <v-expansion-panel-content>
            <v-row v-for="(pyHis,j) in rcpt.paymentHisList" :key="j"
               no-gutters class="rounded mt-1" align="center"
            >
              <div style="width:180px" class="">
                会計：{{pyHis.paymentNumber}}
              </div>
              <div style="width:180px" class="">
                会計金額：{{pyHis.paymentConfirmPriceStr}}
              </div>
              <v-spacer></v-spacer>
              <v-btn dark class="blue darken-3"
                @click="doPrint(rcpt.receptionNumber,pyHis.paymentNumber,true,false)"
              >
                レシート
              </v-btn>
              <v-btn dark class="blue darken-3 ml-2"
                @click="doPrint(rcpt.receptionNumber,pyHis.paymentNumber,false,true)"
              >
                領収書
              </v-btn>
            </v-row>
            <v-row v-if="rcpt.paymentStatus>0" no-gutters class="mt-2">
              <v-spacer></v-spacer>
              <v-btn v-if="rcpt.paymentStatus===1 || rcpt.paymentStatus===2"
                dark  color="orange"
                @click="gotoCashire(rcpt.receptionNumber)"
              >
                <v-icon>mdi-cash-register</v-icon>
                会計
              </v-btn>
              <v-btn  v-if="rcpt.paymentStatus>=2"
              dark color="red" class="ml-2"
                @click="()=>{
                  confirmMemoReceptionNumber=rcpt.receptionNumber
                  confirmMemoPaymentHisList = rcpt.paymentHisList
                  confirmDialgFlg=true
                }"
              >
                <v-icon>mdi-cash-register</v-icon>
                再会計
              </v-btn>
              <v-spacer></v-spacer>
            </v-row>
          </v-expansion-panel-content>
        </v-expansion-panel>
      </v-expansion-panels>
    </div>
    <v-dialog v-model="confirmDialgFlg" persistent max-width="290">
      <v-card>
        <v-card-title class="text-h5">
          再会計確認
        </v-card-title>
        <v-card-text>会計を<i class="red--text">削除</i>して、再会計にしますか？</v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="red darken-1" text
            @click="confirmDialgFlg = false"
          >
            キャンセル
          </v-btn>
          <v-btn color="green darken-1" text
            @click="reCashier()"
          >
            確認
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>
<script>
import SysConst from '../lwUtils/SysConst'
import moment from 'moment'
export default {
  name: 'PaymentHistory',
  data: () => ({
    receiptDateFlg: false,
    receiptDate: null,
    receiptionHisList: [],
    confirmDialgFlg: false,
    confirmMemoReceptionNumber: '',
    confirmMemoPaymentHisList: []
  }),
  watch: {
    receiptDate: {
      handler (val, oldVal) {
        this.getDailyReceiptHistory()
      },
      deep: true
    }
  },
  methods: {
    async initForm () {
      this.receiptDate = moment().format('YYYY-MM-DD')
    },
    async getDailyReceiptHistory () {
      this.receiptionHisList.splice(0)
      const postBody = {
        storeNumber: SysConst.storeNumber,
        receiptDate: this.receiptDate
      }
      const response = await this.$lwHttp.postAsync('pymtHis/getDailyReceiptHistory', postBody)
      this.receiptionHisList.push(...response.receiptionHisList)
    },
    closeBtnClick: function () {
      this.$emit('paymentHistoryClose')
    },
    /**
     * 会計を続き
     */
    gotoCashire (receptionNumber) {
      this.$router.push({ path: `/cashier/${receptionNumber}` })
    },
    /**
     * 再会計
     */
    async reCashier () {
      this.confirmDialgFlg = false

      var postBody = {
        storeNumber: SysConst.storeNumber,
        paymentHisList: []
      }
      this.confirmMemoPaymentHisList.forEach(paym => {
        postBody.paymentHisList.push(paym.paymentNumber)
      })

      const delRpnData =
        await this.$lwHttp.postAsync(
          'pymtHis/deletePaymentsFromOrder',
          postBody
        )
      if (delRpnData.status === 200) {
        this.$router.push({ path: `/cashier/${this.confirmMemoReceptionNumber}` })
      }
    },
    /**
     * レシート・帳票印刷
     */
    async doPrint (receptionNumber, paymentNumber, withReceipt, withReceiptChit) {
      const postBody = {
        storeNumber: SysConst.storeNumber,
        receptionNumber: receptionNumber,
        paymentNumber: paymentNumber,
        withReceipt: withReceipt,
        withReceiptChit: withReceiptChit
      }
      var printSuccessAll = true

      if (withReceipt || withReceiptChit) {
        const printDataResponse = await this.$lwHttp.postAsync(
          'print/getCashierChitPrintDataByPayment',
          postBody
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
    }
  }
}
</script>
