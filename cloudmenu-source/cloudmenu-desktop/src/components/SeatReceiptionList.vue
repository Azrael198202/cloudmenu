<template>
  <div>
    <v-card>
      <v-card-title>
        <v-row :align="'center'">
          <v-col cols="6">受付一覧</v-col>
          <v-col cols="6" class="text-right">
            <v-btn icon color="red"
              @click="closeBtnClicked()"
            >
              <v-icon large>mdi-close-circle</v-icon>
            </v-btn>
          </v-col>
        </v-row>
      </v-card-title>
      <v-divider></v-divider>
      <v-card-text style="min-height:500px;">
        <v-container>
          <div v-for="(seatInfo,index) in seatSumInfoList" :key="index">
            <div v-if="!!seatInfo.receptionNumber && seatInfo.receptionBranchNumber ===1">
            <v-row>
              <v-col cols="5">
                <div><span class="grey--text">受付番号</span>：<span class="font-weight-bold">{{seatInfo.receptionNumber}}</span></div>
                <div><span class="grey--text" style="padding-right:28px">座席</span>：<span class="font-weight-bold">{{seatInfo.seatMergeAll}}</span></div>
              </v-col>
              <v-col cols="3">
                <div><span class="grey--text">注文時間</span>：<span class="font-weight-bold">{{seatInfo.seatStartDateHm}}</span></div>
                <div><span class="grey--text">受取時間</span>：<span class="font-weight-bold">{{seatInfo.takeoutReceiptTimeHm}}</span></div>
              </v-col>
              <v-col cols="2" class="text-right"
                @click="recpCashierClicked(seatInfo.receptionNumber)"
              >
                <v-btn icon color="orange darken-2">
                  <v-icon large>mdi-cash-register</v-icon>
                </v-btn>
              </v-col>
              <v-col cols="2" class="text-right"
                @click="deleteClicked(seatInfo.orderedAllCount,seatInfo.receptionNumber)"
              >
                <v-btn icon color="red darken-2">
                  <v-icon large>mdi-signature-freehand</v-icon>
                </v-btn>
              </v-col>
            </v-row>
            <v-divider></v-divider>
            </div>
          </div>
        </v-container>
      </v-card-text>
      <v-divider></v-divider>
    </v-card>
    <v-dialog persistent width="825px"
      v-model="signDialogFlg">
      <sign-panel
        ref="signPnl"
        @cancelClicked = "signDialogFlg=false"
        @signatureSaved = "signatureSaved"
      >
      </sign-panel>
    </v-dialog>
  </div>
</template>

<script>
import SignPanel from './SignPanel'

export default {
  name: 'SeatReceiptionList',
  components: {
    SignPanel
  },
  props: {
    seatSumInfoList: {
      type: Array,
      default: function () {
        return []
      }
    }
  },
  data: () => ({
    signDialogFlg: false
  }),
  methods: {
    closeBtnClicked () {
      this.$emit('closeBtnClicked')
    },
    recpDetailClicked (receptionNumber) {
      this.$emit('recpDetailClicked', receptionNumber)
    },
    recpCashierClicked (receptionNumber) {
      this.$emit('recpCashierClicked', receptionNumber)
    },
    deleteClicked (orderedAllCount, receptionNumber) {
      if (orderedAllCount) {
        this.signDialogFlg = true
        this.$nextTick(() => {
          this.$refs.signPnl.initForm(receptionNumber)
        })
      } else {
        this.$emit('deleteClicked', receptionNumber)
      }
    },
    signatureSaved (receptionNumber) {
      this.$emit('deleteClicked', receptionNumber)
    }
  }
}
</script>
