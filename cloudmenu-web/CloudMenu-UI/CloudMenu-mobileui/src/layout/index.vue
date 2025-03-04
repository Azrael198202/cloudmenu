<template>
  <div id="app">
    <commonHeader
      :show-left="showLeft"
      :show-header="showHeader"
      :table-num="seatNumber"
      :show-back="showBack"
    />
    <!-- 导航结束 -->
    <div class="body">
      <router-view :key="key" />
    </div>
    <commonFooter v-if="showFooter" :show-index="showIndex" />
  </div>
</template>

<script>
import commonHeader from '@/layout/commonHeader.vue'
import commonFooter from '@/layout/commonFooter.vue'
export default {
  name: 'Layout',
  components: {
    commonHeader,
    commonFooter
  },
  data() {
    return {
      showError: true,
      message: '座席データ全件検索でエラーが発生しました。',
      showFooter: false,
      showIndex: false,
      showLeft: false,
      showBack: false,
      showHeader: false,
      seatNumber: ''
    }
  },
  computed: {
    key() {
      return this.$route.name !== undefined ? this.$route.name + new Date() : this.$route + new Date()
    }
  },
  watch: {
    $route(to, from) {
      // 对路由变化作出响应...
      var meta = to.meta
      if (to.query.orderInfo) {
        this.seatNumber = to.query.orderInfo.seatNumber
      }
      this.showFooter = meta.showFooter
      this.showIndex = meta.showIndex
      this.showLeft = meta.showLeft
      this.showBack = meta.showBack
      this.showHeader = meta.showHeader
    }
  },
  created() {
    var meta = this.$route.meta
    this.showFooter = meta.showFooter
    this.showIndex = meta.showIndex
    this.showLeft = meta.showLeft
    this.showBack = meta.showBack
    this.showHeader = meta.showHeader
  },
  methods: {}
}
</script>

<style lang="scss" scoped>
.body {
  // margin-top: 96px;
  // margin-bottom: 40px;
}
</style>
