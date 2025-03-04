<template>
  <div dark class="grey darken-4 white--text" style="width:1903px;border: 2px solid #ddd;user-select:none;">
    <!-- ***Header************************************************************************************************************** -->
    <v-container fluid class="text-h5 font-weight-blod" style="position:sticky;top:0px;">
      <v-row no-gutters align="center" class="">
        <v-col cols="4" class="text-left pl-5" @click="routeBack()">
          <v-icon large class="mr-2" color="red">mdi-backburger</v-icon>{{SysConst.storeName}}
        </v-col>
        <v-col cols="4" class="text-center">入出金</v-col>
        <v-col cols="4" class="text-right pr-5">{{nowStrJp}}</v-col>
      </v-row>
    </v-container>
    <!-- ***Body************************************************************************************************************** -->
    <v-container fluid class="" style="height:926px">
      <v-row no-gutters style="height:100%">
        <!-- ***Body-Left************************************************************************************************************** -->
        <v-col cols="4" class="text-subtitle-1">
          <v-form ref="form" v-model="valid">
            <v-container class="grey darken-3 rounded-lg">
              <v-row no-gutters align="center">
                <v-img max-height="45" max-width="45" src="/img/stuff.png" style="display:inline-block"></v-img>
                <span>担当者</span>
                <v-spacer></v-spacer>
                <span>老上海</span>
              </v-row >
            </v-container>
            <v-container class="grey darken-3 rounded-lg mt-3">
              <v-row no-gutters align="center">
                <v-col cols="3" class="">
                  <v-icon x-large dark
                    color="blue darken-2"
                  >
                    mdi-calendar-month
                  </v-icon>
                  <span class="ml-1">日付</span>
                </v-col>
                <v-col>
                  <v-menu dark
                    v-model="moneyInoutFlg"
                    :close-on-content-click="false"
                    :nudge-right="40"
                    transition="scale-transition"
                    offset-y
                    min-width="auto"
                  >
                    <template v-slot:activator="{ on, attrs }">
                      <v-text-field dark
                        v-model="moneyInoutDate"
                        readonly
                        v-bind="attrs"
                        v-on="on"
                      ></v-text-field>
                    </template>
                    <v-date-picker
                      v-model="moneyInoutDate"
                      @input="moneyInoutFlg = false"
                      locale="ja"
                    ></v-date-picker>
                  </v-menu>
                </v-col>
              </v-row >
            </v-container>
            <v-container class="grey darken-3 rounded-lg mt-3">
              <v-row no-gutters align="center">
                <v-col cols="3" class="pl-11">
                  <span>区分</span>
                </v-col>
                <v-col>
                  <v-radio-group v-model="moneyInouKbn" row dark
                  >
                    <v-radio
                      label="入金"
                      value="300"
                    ></v-radio>
                    <v-radio
                      label="出金"
                      value="301"
                    ></v-radio>
                  </v-radio-group>
                </v-col>
              </v-row >
            </v-container>
            <v-container class="grey darken-3 rounded-lg mt-3">
              <v-row no-gutters >
                <v-col cols="3" class="pl-11">
                  <span>金額</span>
                </v-col>
                <v-col class="">
                  <v-text-field outlined dense dark
                    required
                    :rules="moneyRules"
                    type="number"
                    prefix="¥"
                    v-model="moneyInoutPrice"
                  ></v-text-field>
                  <!-- <div class="grey darken-2 rounded-lg" style="height:35px;"></div> -->
                </v-col>
              </v-row >
            </v-container>
            <v-container class="grey darken-3 rounded-lg mt-3">
              <v-row no-gutters>
                <v-col cols="3" class="pl-11">
                  <span>入金理由</span>
                </v-col>
                <v-col>
                  <v-select solo
                    v-model="moneyInoutReasonKbn"
                    :items="moneyInOutReasonKbnItems"
                  ></v-select>
                </v-col>
              </v-row >
            </v-container>
            <v-container class="grey darken-3 rounded-lg mt-3">
              <v-row no-gutters>
                <v-col cols="3" class="pl-11">
                  <span>備考</span>
                </v-col>
                <v-col>
                  <v-textarea dark
                    rows="10"
                    filled
                    v-model="moneyInoutRemark"
                  ></v-textarea>
                </v-col>
              </v-row >
            </v-container>
            <v-btn dark block x-large class="mt-2 red darken-3 text-h5"
              @click="addMoneyBtnClick()"
            >
              確定
            </v-btn>
          </v-form>
        </v-col>
        <!-- ***Body-Right************************************************************************************************************** -->
        <v-col cols="8" class="text-h6 pl-5">
          <v-container class="grey darken-3 rounded-lg pa-2">
            <v-row no-gutters align="center">
              <div class="" style="width:150px">
                No
              </div>
              <div class="" style="width:140px">
                日付
              </div>
              <div class="" style="width:90px">
                区分
              </div>
              <div class="" style="width:120px">
                担当者
              </div>
              <div class="text-right pr-4" style="width:150px">
                金額
              </div>
              <div class="pl-2" style="width:200px">
                理由
              </div>
              <div class="" style="width:280px">
                備考
              </div>
              <div class="text-center" style="width:80px">
                操作
              </div>
            </v-row >
          </v-container>
          <v-virtual-scroll class="mt-4"
            :items="moneyInOutDataList"
            height="830px"
            item-height="65"
          >
            <template v-slot:default="{ item }">
              <v-container class="white darken-3 rounded-lg pa-2 black--text text-subtitle-1">
                <v-row no-gutters align="center">
                  <div class="" style="width:150px">
                    {{item.moneyInoutNumber}}
                  </div>
                  <!-- 日付 -->
                  <div class="" style="width:140px">
                    {{item.moneyInoutDateStr}}
                  </div>
                  <!-- 区分 -->
                  <div class="" style="width:90px">
                    {{item.moneyInoutKbnName}}
                  </div>
                  <div class="" style="width:120px">
                    老上海
                  </div>
                  <!-- 金額 -->
                  <div class="text-right pr-4" style="width:150px">
                    {{jpCurrencyFmt.format(item.moneyInoutPrice)}}
                  </div>
                  <!-- 理由 -->
                  <div class="pl-2" style="width:200px">
                    {{item.moneyInoutReasonKbnName}}
                  </div>
                  <!-- 備考 -->
                  <div class="" style="width:280px">
                    {{item.moneyInoutRemarks}}
                  </div>
                  <div class="text-center" style="width:80px">
                    <v-btn class="mx-2" fab dark small color="red"
                      @click="deleteMoneyBtnClick(item)"
                    >
                      <v-icon dark>mdi-minus</v-icon>
                    </v-btn>
                  </div>
                </v-row >
              </v-container>
            </template>
          </v-virtual-scroll>
        </v-col>
      </v-row >
    </v-container>
    <!-- ***Dialog************************************************************************************************************** -->
    <v-dialog v-model="moneyConfirmDialog" persistent max-width="400">
      <v-card>
        <v-card-title class="text-h5">
          {{moneyInoutKbnName}}登録確認
        </v-card-title>
        <v-card-text>
          {{jpCurrencyFmt.format(moneyInoutPrice)}} の{{moneyInoutKbnName}} を登録してよろしいですか？
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="green darken-1"
            text
            @click="addMoneyInOut(true)"
          >
            登録する
          </v-btn>
          <v-btn color="red darken-1"
            text
            @click="moneyConfirmDialog = false"
          >
            キャンセル
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    <!-- ***Dialog************************************************************************************************************** -->
    <v-dialog v-model="moneyDeleteDialog.flg" persistent max-width="400">
      <v-card>
        <v-card-title class="text-h5">
          {{moneyDeleteDialog.title}}
        </v-card-title>
        <v-card-text>
          {{moneyDeleteDialog.content}}
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="green darken-1"
            text
            @click="deleteMoneyInOut()"
          >
            削除する
          </v-btn>
          <v-btn color="red darken-1"
            text
            @click="moneyDeleteDialog.flg = false"
          >
            キャンセル
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
import SysConst from '../lwUtils/SysConst'
import moment from 'moment'
const jpCurrencyFmt = new Intl.NumberFormat('ja-JP', { style: 'currency', currency: 'JPY' })

export default {
  name: 'MoneyInOut',
  components: {
  },
  data: () => ({
    SysConst: SysConst,
    // 日本金額フォマード
    jpCurrencyFmt: jpCurrencyFmt,
    // システム日時 eg 2021/05/17(月) 12:30
    nowStrJp: '',
    // ------------------------------------------
    moneyInoutFlg: false,
    moneyInoutDate: '',
    moneyInouKbn: '300',
    moneyInoutReasonKbn: '001',
    moneyInoutPrice: null,
    moneyInoutRemark: '',
    moneyInReasonKbns: [{ value: '001', text: 'おつり用で入金' }],
    moneyOutReasonKbns: [{ value: '001', text: '小口用で出金' }],
    moneyInOutDataList: [],
    // ----------------------------------------
    valid: true,
    moneyRules: [
      v => !!v || '金額を入力してください。',
      v => (v && v > 0) || 'マイナス金額が入力出来ません。'
    ],
    // ----------------------------------------
    moneyConfirmDialog: false,
    // ----------------------------------------
    moneyDeleteDialog: {
      flg: false,
      moneyInoutNumber: '',
      title: '',
      content: ''
    }
  }),
  computed: {
    moneyInOutReasonKbnItems () {
      return this.moneyInouKbn === '300' ? this.moneyInReasonKbns : this.moneyOutReasonKbns
    },
    moneyInoutKbnName () {
      return this.moneyInouKbn === '300' ? '入金' : '出金'
    }
  },
  async mounted () {
    moment.locale('ja')
    this.nowStrJp = moment().format('llll')
    this.initMoneyData()
    this.moneyInoutDate = moment().format('YYYY-MM-DD')
    setInterval(this.refeshSysDateTime, 3500)
  },
  watch: {
    moneyInoutDate: {
      handler (val, oldVal) {
        this.getDailyMoneyInOutData()
      },
      deep: true
    }
  },
  methods: {
    refeshSysDateTime () {
      // システム日付
      this.nowStrJp = moment().format('llll')
    },
    routeBack () {
      this.$router.go(-1)
    },
    initMoneyData () {
      this.moneyInouKbn = '300'
      this.moneyInoutReasonKbn = '001'
      this.moneyInoutPrice = null
      this.moneyInoutRemark = ''
    },
    async getDailyMoneyInOutData () {
      console.log('getDailyMoneyInOutData', this.moneyInoutDate)
      const response = await this.$lwHttp.postAsync('moneyinout/getDailyMoneyInOut', { storeNumber: SysConst.storeNumber, MoneyInoutDatetime: this.moneyInoutDate })
      this.moneyInOutDataList.splice(0)
      this.moneyInOutDataList.push(...response.moneyInOutList)
    },
    addMoneyBtnClick () {
      if (this.$refs.form.validate()) {
        this.moneyConfirmDialog = true
      }
    },
    async addMoneyInOut () {
      const postBody = {}
      postBody.storeNumber = SysConst.storeNumber
      postBody.MoneyInoutDatetime = this.moneyInoutDate
      postBody.MoneyInoutPrice = this.moneyInoutPrice
      postBody.MoneyInoutKbn = this.moneyInouKbn
      postBody.MoneyInoutReasonKbn = this.moneyInoutReasonKbn
      postBody.MoneyInoutRemarks = this.moneyInoutRemark

      const response = await this.$lwHttp.postAsync('moneyinout/addMoneyInOut', postBody)
      this.moneyInOutDataList.splice(0)
      this.moneyInOutDataList.push(...response.moneyInOutList)

      this.initMoneyData()
      this.moneyConfirmDialog = false
    },
    deleteMoneyBtnClick (moneyInoutData) {
      this.moneyDeleteDialog.title = `${moneyInoutData.moneyInoutKbnName}削除確認`
      this.moneyDeleteDialog.content = `${moneyInoutData.moneyInoutNumber}の${moneyInoutData.moneyInoutKbnName}を削除してよろしいですか?`
      this.moneyDeleteDialog.moneyInoutNumber = moneyInoutData.moneyInoutNumber
      this.moneyDeleteDialog.flg = true
    },
    async deleteMoneyInOut () {
      const postBody = {}
      postBody.storeNumber = SysConst.storeNumber
      postBody.MoneyInoutDatetime = this.moneyInoutDate
      postBody.moneyInoutNumber = this.moneyDeleteDialog.moneyInoutNumber

      const response = await this.$lwHttp.postAsync('moneyinout/delMoneyInOut', postBody)
      this.moneyInOutDataList.splice(0)
      this.moneyInOutDataList.push(...response.moneyInOutList)

      this.initMoneyData()
      this.moneyDeleteDialog.flg = false
    }
  }
}
</script>
