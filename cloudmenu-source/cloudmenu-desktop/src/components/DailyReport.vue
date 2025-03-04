<template>
  <div>
    <v-container fluid class="white" style="user-select:none">
      <v-form ref="form" v-model="valid">
      <v-row align="center" class=" grey darken-3 white--text pr-2">
        <v-col class="pl-6 text-h6">
          日計レポート出力
        </v-col>
        <v-spacer></v-spacer>
        <v-btn icon color="red"
          @click="closeBtnClick()"
        >
          <v-icon large>mdi-close-circle</v-icon>
        </v-btn>
      </v-row>
      <v-row class="pl-4 mt-6">
        <v-col cols="2">
          <v-switch
            v-model="isMonth"
            :label="isMonth ? '月別':'日別'"
            @change="dailyTypeChange"
          ></v-switch>
        </v-col>
        <v-col cols="10">
          <v-menu
            v-model="reportDateFlg"
            :close-on-content-click="false"
            :nudge-right="40"
            transition="scale-transition"
            offset-y
            min-width="auto"
          >
            <template v-slot:activator="{ on, attrs }">
              <v-text-field
                prepend-icon="mdi-calendar"
                :label="isMonth ? 'レポート月' : 'レポート日'"
                v-model="reportDate"
                readonly
                v-bind="attrs"
                v-on="on"
              ></v-text-field>
            </template>
            <v-date-picker
              v-model="reportDate"
              @input="reportDateFlg = false"
              :type="isMonth ? 'month' : 'date'"
              locale="ja"
            ></v-date-picker>
          </v-menu>
        </v-col>
      </v-row>
      <v-row align="center" justify="space-around">
        <v-checkbox
          v-model="reportTypes"
          label="取引別"
          value="1"
        ></v-checkbox>
        <v-checkbox
          v-model="reportTypes"
          label="部門別"
          value="2"
        ></v-checkbox>
        <v-checkbox
          v-model="reportTypes"
          label="分類別"
          value="3"
        ></v-checkbox>
        <v-checkbox
          v-model="reportTypes"
          label="会計取引種別"
          value="4"
        ></v-checkbox>
        <v-checkbox
          v-model="reportTypes"
          label="商品別"
          value="5"
        ></v-checkbox>
      </v-row>
      <v-row class="pl-4">
        <div class="red--text">{{errorMsg}}</div>
      </v-row>
      <v-row align="center" justify="space-around">
        <v-btn dark block x-large class="mt-2 red darken-3 text-h5"
          @click="printBtnClick()"
        >
          発行
        </v-btn>
      </v-row>
      </v-form>
    </v-container>
    <v-dialog v-model="chitXmlDialog" persistent max-width="400">
      <v-card>
        <v-card-title class="text-h5"> レシートと領収書の結果 </v-card-title>
        <v-card-text>{{chitXmlText}}</v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="red darken-1" text @click="copyXmlToClipboard()">コピー</v-btn>
          <v-btn color="red darken-4" text @click="chitXmlDialog=false">閉じる</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>
<script>
import SysConst from '../lwUtils/SysConst'
import moment from 'moment'

export default {
  name: 'DailyReport',
  props: {
  },
  data: () => ({
    valid: true,
    isMonth: false,
    reportDateFlg: false,
    reportDate: null,
    reportTypes: [],
    errorMsg: '',
    // ----------------------------------
    chitXmlDialog: false,
    chitXmlText: ''
  }),
  mounted () {
    moment.locale('ja')
  },
  methods: {
    initForm () {
      this.isMonth = false
      this.reportDate = moment().format('YYYY-MM-DD')
      this.reportTypes = ['1']
    },
    dailyTypeChange () {
      if (this.isMonth) {
        this.reportDate = moment().format('YYYY-MM')
      } else {
        this.reportDate = moment().format('YYYY-MM-DD')
      }
    },
    closeBtnClick: function () {
      this.$emit('dailyReportClose')
    },
    async printBtnClick () {
      this.errorMsg = ''

      if (!this.reportDate) {
        this.errorMsg = '日付を選択してください。'
        return
      }

      if (this.reportTypes.length <= 0) {
        this.errorMsg = 'レポート種別を選択してください。'
        return
      }
      // tochange
      var printRslt = await this.doPrint()
      // var printRslt = await this.doPrintTest()
      if (printRslt) {
        this.$emit('dailyReportClose')
      }
    },
    async doPrint () {
      const postData =
        {
          storeNumber: SysConst.storeNumber,
          reportIsMonth: this.isMonth,
          reportDate: this.reportDate,
          reportTypes: this.reportTypes
        }

      const printDataResponse = await this.$lwHttp.postAsync('print/getDailyReportsData', postData)
      if (printDataResponse.status === 204) {
        this.$toasted.show('該当データがない。', {
          theme: 'bubble',
          position: 'top-center',
          duration: 5000
        })
        return false
      }

      var printSuccessAll = true
      var printerDataList = printDataResponse.printerDataList

      for (var pntIdx = 0; pntIdx < printerDataList.length; pntIdx++) {
        var printerData = printerDataList[pntIdx]
        var printXmlDatas = printerData.printXmlDatas

        for (let xmlIdx = 0; xmlIdx < printXmlDatas.length; xmlIdx++) {
          const xmlStr = printXmlDatas[xmlIdx]
          var printRslt = await this.$lwHttp.printChit(printerData.printerUri, xmlStr)
          var printSuccess = printRslt.success

          printSuccessAll = printSuccessAll && (printSuccess === 'true')
          if (printSuccess === 'false') {
            var printerName = printerData.printerName
            var msgCode = printRslt.code
            var msgStr = printRslt.message
            this.$toasted.error(`[プリンター${printerName}] 印刷エラー [${msgCode}]：${msgStr}`,
              { theme: 'bubble', position: 'top-center', duration: 8000, icon: 'print_disabled' }
            )
          }
        }
      }

      return printSuccessAll
    },
    async doPrintTest () {
      const postData =
        {
          storeNumber: SysConst.storeNumber,
          reportDate: this.reportDate,
          reportTypes: this.reportTypes
        }
      const printDataResponse = await this.$lwHttp.postAsync('print/getDailyReportsData', postData)
      if (printDataResponse.status === 204) {
        this.$toasted.show('該当データがない。', {
          theme: 'bubble',
          position: 'top-center',
          duration: 5000
        })
        return false
      }

      // var printSuccessAll = true
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

      return false
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
