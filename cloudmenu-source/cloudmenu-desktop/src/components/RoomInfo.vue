<template>
  <div class="room-info rounded-lg pa-1"
      v-bind:class="cmpColorClass"
      v-bind:style="{
        top: seatSumInfo.seatPositionVertical + 'px' ,
        left:seatSumInfo.seatPositionHorizontal +'px' ,
        width:seatSumInfo.seatSizeHorizontal +'px',
        height:seatSumInfo.seatSizeVertical +'px'
      }"
      @click="emitClick"
      v-longclick="showMenu"
    >
    <div v-if="!seatSumInfo.seatStatusNo" class="text-center pt-9 white--text" style="height:100%;">
          {{seatSumInfo.seatName}}
    </div>
    <div v-else>
      <div style="height:30px" class="text-center white--text">
          {{seatSumInfo.seatName}}
      </div>
      <div v-if="seatSumInfo.seatMergeWith" style="height:72px" class="grey white--text pt-3 subtitle-2 rounded-lg text-center">
          {{seatSumInfo.seatMergeWith}}
      </div>
      <div v-else style="height:72px" class="white rounded-lg text-center">
        <div style="height:45px;" class="pt-4 font-weight-bold"
          v-bind:style="{fontSize: cmPRoomMainInfoFontSize}" >
          {{cmPRoomMainInfo}}
        </div>
        <div class="text-right pr-1"
          v-bind:class="cmpTextColorClass"
        >
          {{seatSumInfo.seatPelpleAll}}名
        </div>
      </div>
    </div>

    <v-menu
      v-model="menuFlg"
      :position-x="seatSumInfo.seatPositionHorizontal + 25"
      :position-y="seatSumInfo.seatPositionVertical + 220"
      absolute
      offset-y
    >
      <v-list>
        <v-list-item @click="cashierMenuClick()">
          <v-list-item-icon>
            <v-icon color="orange darken-2">mdi-cash-register</v-icon>
          </v-list-item-icon>
          <v-list-item-content>
            <v-list-item-title>会計レジ</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list>
    </v-menu>
  </div>
</template>

<script>
const jpCurrencyFmt = new Intl.NumberFormat('ja-JP', { style: 'currency', currency: 'JPY' })
export default {
  name: 'RoomInfo',
  props: {
    seatSumInfo: {
      type: Object,
      default: function () {
        return {
          seatName: '',
          // サイズ縦
          seatSizeVertical: 110,
          // サイズ横
          seatSizeHorizontal: 110,
          // 座席位置縦
          seatPositionVertical: 80,
          // 座席位置横
          seatPositionHorizontal: 80,
          /**
           * 0:空き
           * 1:注文中
           * 2:配膳中
           * 3:待ちなし
           * 4:通知メッセージ有り
           */
          seatStatusNo: 0,
          seatPelpleAll: 0,
          orderPriceSum: 0,
          seatMergeWith: '',
          receptionNumber: ''
        }
      }
    }
  },
  computed: {
    cmpColorClass: function () {
      let colorCls = ''
      if (this.seatSumInfo.seatStatusNo === 0) {
        colorCls = 'grey lighten-1'
      } else if (this.seatSumInfo.seatStatusNo === 1) {
        colorCls = 'blue'
      } else if (this.seatSumInfo.seatStatusNo === 2) {
        colorCls = 'orange'
      } else if (this.seatSumInfo.seatStatusNo === 3) {
        colorCls = 'light-green'
      } else if (this.seatSumInfo.seatStatusNo === 4) {
        colorCls = 'flashing'
      } else {
        colorCls = 'grey lighten-1'
      }
      return colorCls
    },
    cmpTextColorClass: function () {
      let txtColCls = ''
      if (this.seatSumInfo.seatStatusNo === 0) {
        txtColCls = ''
      } else if (this.seatSumInfo.seatStatusNo === 1) {
        txtColCls = 'blue--text'
      } else if (this.seatSumInfo.seatStatusNo === 2) {
        txtColCls = 'orange--text'
      } else if (this.seatSumInfo.seatStatusNo === 3) {
        txtColCls = 'light-green--text'
      } else if (this.seatSumInfo.seatStatusNo === 4) {
        txtColCls = 'red--text'
      } else {
        txtColCls = ''
      }
      return txtColCls
    },
    cmPRoomMainInfo: function () {
      let mainInfo = ''
      if (this.seatSumInfo.seatStatusNo === 0) {
        mainInfo = ''
      } else if (this.seatSumInfo.seatStatusNo === 1) {
        mainInfo = '注文中'
      } else if (this.seatSumInfo.seatStatusNo === 2) {
        mainInfo = '配膳中'
      } else if (this.seatSumInfo.seatStatusNo === 3) {
        mainInfo = jpCurrencyFmt.format(this.seatSumInfo.orderPriceSum)
        // mainInfo = this.seatSumInfo.orderPriceSum
      } else if (this.seatSumInfo.seatStatusNo === 4) {
        mainInfo = '配膳中'
      }
      return mainInfo
    },
    cmPRoomMainInfoFontSize: function () {
      let fontSize = '18px'
      if (this.seatSumInfo.seatStatusNo === 3 && this.seatSumInfo.seatKbn === '001') {
        fontSize = '12px'
      }
      return fontSize
    }
  },
  data: () => ({
    menuFlg: false
  }),
  methods: {
    emitClick: function () {
      this.$emit('seatInfoClicked', this.seatSumInfo.receptionNumber)
    },
    showMenu: function (event) {
      // event.preventDefault()
      // this.menuFlg = false
      // this.menuPx = event.clientX
      // this.menuPy = event.clientY
      // this.$nextTick(() => {
      // this.menuFlg = true
      // })

      if (this.seatSumInfo.seatStatusNo > 1) {
        this.menuFlg = true
      }
    },
    cashierMenuClick () {
      if (this.seatSumInfo.receptionNumber) {
        this.$emit('cashierMenuClick', this.seatSumInfo.receptionNumber)
        // this.$router.replace({ path: `/cashier/${this.seatSumInfo.receptionNumber}` })
        // this.$router.replace({ path: 'cashier', params: { recpnumber: this.seatSumInfo.receptionNumber } })
      }
    }
  }
}
</script>
<style scoped>
  .room-info{
    position: absolute;
    width: 110px;
    height: 110px;
    user-select:none;
    cursor: pointer;
  }
  .flashing {
    background-color:orange;
    animation-name: flashingAnima;
    animation-duration: 2s;
    animation-iteration-count: infinite;
  }
  @keyframes flashingAnima {
    50% {background-color:red;}
  }
</style>
