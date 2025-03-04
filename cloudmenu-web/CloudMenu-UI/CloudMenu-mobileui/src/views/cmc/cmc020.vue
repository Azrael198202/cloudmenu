<template>
  <div class="page-index">
    <van-row class="top">
      <i class="header-help" @click="help()" />
    </van-row>
    <van-row class="page-index-main">
      <van-row class="logo" style="margin-bottom:20px;"><img :src="require('@/assets' + logoImage)" alt="" /></van-row>
      <van-row class="introduction-title">{{ introduction }}</van-row>
      <van-row class="page-index-img"><img :src="require('@/assets' + image)" alt="" /></van-row>
    </van-row>
  </div>
</template>
<script>
import { searchStoreInfo } from '@/api/cmc/cmc020'
export default {
  name: 'CustomerMenu',
  data() {
    return {
      active: '',
      introduction: '',
      showError: false,
      storeList: [],
      logoImage: '',
      image: ''
    }
  },
  created() {
    this.searchStoreInfo()
  },
  methods: {
    // 初始化页面调取数据
    searchStoreInfo() {
      searchStoreInfo().then(response => {
        if (response.status === 200) {
          this.storeList = response.storeList
          this.introduction = this.storeList[0].introduction
          this.logoImage = this.storeList[0].logoImage
          this.image = this.storeList[0].image
        } else if (response.status === 601) {
          this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
        } else if (response.status === 602) {
          this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
        }
      })
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
<style scoped lang="scss">
.page-index {
  .top {
    height: 72px;
    position: relative;
  }

  .page-index-main {
    display: flex;
    justify-content: center;
    flex-wrap: wrap;
    padding: 0 40px;

    .logo {
      width: 270px;
      margin-top: 20px; // logo图带阴影，很大

      img {
        width: 100%;
      }
    }

    .introduction-title {
      font-size: 12px;
      overflow: hidden;
      display: -webkit-box;
      -webkit-box-orient: vertical;
      -webkit-line-clamp: 2;
      height: 36px;
      line-height: 18px;
      opacity: 0.6;
    }

    .page-index-img {
      width: 100%;
      margin-top: 40px;

      img {
        width: 100%;
        height: auto;
      }
    }
  }
}
</style>
