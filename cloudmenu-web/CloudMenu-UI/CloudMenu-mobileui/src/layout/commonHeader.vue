<template>
  <div>
    <van-row v-if="showHeader" class="wrap-header">
      <van-row v-show="showLeft" class="common-header">
        <van-row class="left">
          <div>
            <span>{{ seatName }}</span>
            <span />
          </div>
        </van-row>
        <img src="../assets/images/logo.png" alt="" />
        <i class="header-help" @click="help()" />
      </van-row>

      <van-row v-show="!showLeft && !showBack" class="common-header">
        <img src="../assets/images/logo.png" alt="" />
        <i class="header-help" @click="help()" />
      </van-row>

      <van-row v-show="showBack" :gutter="24" type="flex" justify="space-around" class="common-header">
        <van-col :span="6" class="arrow">
          <van-icon name="arrow-left" size="25px" @click="back()" />
        </van-col>
        <van-col :span="6">
          <img src="../assets/images/logo.png" alt="" />
        </van-col>
        <van-col>
          <i class="header-help" @click="help()" />
        </van-col>
      </van-row>
    </van-row>
  </div>
</template>

<script>
export default {
  name: 'CommonHeader',
  components: {},
  props: {
    showLeft: {
      type: Boolean,
      default: null
    },
    tableNum: {
      type: String,
      default: null
    },
    showBack: {
      type: Boolean,
      default: null
    },
    showHeader: {
      type: Boolean,
      default: null
    }
  },
  data() {
    return {
      seatName: '',
      seatNumber: '',
      numberOfPeople: '',
      menuOrderNumber: '',
      menuClassNumber: ''
    }
  },
  computed: {},
  created() {
    if (
      sessionStorage.getItem('seatInfo') &&
      sessionStorage.getItem('seatInfo') !== null &&
      sessionStorage.getItem('seatInfo') !== undefined
    ) {
      const queryParam = JSON.parse(sessionStorage.getItem('seatInfo'))
      if (queryParam.receptionKbn === '002') {
        this.seatName = '持ち帰り'
      } else {
        this.seatName = queryParam.mainSeat.seatName
        this.seatNumber = queryParam.mainSeat.seatNumber
      }
    }
  },
  methods: {
    back() {
      this.$router.back()
    },
    help() {
      // 获取当前窗口的路径
      var pathname = window.location.pathname
      if (pathname.indexOf('Employee') !== -1) {
        // 店员的pdf帮助说明
        window.open(
          'https://www.sato.co.jp/support/printertool/tool/' +
            'multi-labelist-component/pdf/MLCMP-Manual-Reference-v5_9_5_0_r210121.pdf'
        )
      } else if (pathname.indexOf('dashboard') !== -1 || pathname.indexOf('Customer') !== -1) {
        // 顾客的pdf帮助说明
        window.open(
          'https://www.sato.co.jp/support/printertool/tool/' +
            'multi-labelist-component/pdf/MLCMP-Manual-Reference-v5_9_5_0_r210121.pdf'
        )
      }
    }
  }
}
</script>

<style lang="scss" scoped>
@import '@/styles/variables.scss';
.wrap-header {
  height: 77px;
}
.common-header {
  background: linear-gradient(to bottom right, $titleBgColorLinear1, $titleBgColorLinear2 100%);
  height: 72px;
  width: 100%;
  border-bottom: 5px transparent solid;
  border-image: linear-gradient(to right, $linearBorder1, $linearBorder2) 1 10;
  position: fixed;
  z-index: 99;
  padding: 0;

  .left {
    width: 66px;
    height: 44px;
    border: 2px solid $mainBorder;
    border-radius: 10px;
    position: absolute;
    top: 50%;
    left: 29px;
    transform: translateY(-50%);
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    color: $titleColor;
    font-size: $smallSize;
    margin: 0;

    span {
      display: block;
      text-align: center;
    }
  }

  img {
    position: absolute;
    left: 50%;
    top: 50%;
    transform: translate(-50%, -50%);
    height: 112px;
  }

  .arrow {
    display: flex;
    align-items: center;
    color: $mainBorder;
  }
}
</style>
