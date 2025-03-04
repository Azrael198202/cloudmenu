<template>
  <div class="footer">
    <van-tabbar v-if="showIndex" v-model="active" class="tab-two">
      <van-tabbar-item @click="toIndexPage">
        <span>ホーム</span>
        <template #icon>
          <img src="@/assets/images/home.png" alt="">
        </template>
      </van-tabbar-item>
      <van-tabbar-item @click="toProductMenu">
        <span>メニュー</span>
        <template #icon>
          <img src="@/assets/images/menu.png" alt="">
        </template>
      </van-tabbar-item>
    </van-tabbar>

    <van-tabbar v-if="!showIndex" v-model="active" :border="border">
      <van-tabbar-item tabindex="100" @click="toSeatSelection">
        <span>座席選択</span>
        <template #icon>
          <img src="@/assets/images/seat.png" alt="">
        </template>
      </van-tabbar-item>
      <van-tabbar-item tabindex="110" @click="toCommoditySearchMethod">
        <span>メニュー</span>
        <template #icon>
          <img src="@/assets/images/menu.png" alt="">
        </template>
      </van-tabbar-item>
      <van-tabbar-item v-if="orderCount===0" tabindex="120" @click="toOrderDecision">
        <span>注文確定</span>
        <template #icon>
          <img src="@/assets/images/order.png" alt="">
        </template>
      </van-tabbar-item>
      <van-tabbar-item v-else tabindex="120" :badge="orderCount" @click="toOrderDecision">
        <span>注文確定</span>
        <template #icon>
          <img src="@/assets/images/order.png" alt="">
        </template>
      </van-tabbar-item>
      <van-tabbar-item tabindex="130" @click="toOrderHistory">
        <span>履歴</span>
        <template #icon>
          <img src="@/assets/images/historical.png" alt="">
        </template>
      </van-tabbar-item>
    </van-tabbar>
  </div>
</template>

<script>
import { getShoppingCart } from '@/utils/auth'
export default {
  name: 'CommonFooter',
  components: {},

  props: {
    showIndex: {
      type: Boolean,
      default: null
    }
  },
  data() {
    return {
      orderCount: 0,
      active: '',
      border: ''
    }
  },
  computed: {},
  watch: {
    '$store.state.user.orderCount': function(newVal) {
      this.orderCount = newVal
    }
  },
  created() {
    const shoppingCartData = getShoppingCart()
    if (shoppingCartData === null || shoppingCartData === undefined) {
      this.orderCount = 0
    } else {
      this.orderCount = shoppingCartData.orderCount
    }
  },
  methods: {
    // 1.首页
    toIndexPage() {
      this.$router.push({ path: '/Customer/CustomerMenu' })
    },
    // 选择坐席
    toSeatSelection() {
      this.$router.push({ path: '/Employee/ReceptionSelection' })
    },
    toOrderDecision() {
      this.$router.push({ path: '/Employee/OrderDecision' })
    },
    toOrderHistory() {
      this.$router.push({ path: '/Employee/OrderHistory' })
    },
    toCommoditySearchMethod() {
      this.$router.push({ path: '/Employee/CommoditySearchMethod' })
    },
    toProductMenu() {
      const pathname = window.location.pathname
      if (pathname.indexOf('/Customer/ProductMenu') !== -1) {
        this.$router.go(0)
      } else {
        this.$router.push({ path: '/Customer/ProductMenu' })
      }
    }
  }
}
</script>

<style lang="scss">
@import '@/styles/variables.scss';
</style>
