<template>
  <div>
    <van-row v-if="message !== null && message !== ''" class="error-message">
      {{ message }}
    </van-row>
    <van-tabs v-model="tabActive" animated class="list menu-list">
      <van-tab v-for="item in menuList" :key="item.menuLineNumber" :name="item.menuClassnumber">
        <template #title>
          <div
            :class="item.menuClassnumber == tabActive ? 'red-Title' : 'normal-Title'"
            @click="menuTypeClick(item.menuLineNumber)"
          >
            {{ item.menuClassName }}
          </div>
        </template>
        <van-list v-model="loading" :finished="finished" @load="onLoad">
          <van-cell-group class="list">
            <van-cell v-for="(obj, index) in newList" :key="index" @click="chooseFood(obj)">
              <template #title>
                <van-row type="flex" justify="space-around" align="center">
                  <!-- 2.无图上架商品-->
                  <van-col v-if="obj.image" span="8" class="list-l" style="width:120px;">
                    <!-- <van-image width="110" height="110" :src="require('@/assets' + obj.image)" /> -->
                    <van-image width="110" height="110" :src="imgUrl + obj.image" />
                    <van-row class="layer">No.{{ obj.menuOrderNumber }}</van-row>
                    <!-- 未上架时加遮罩 -->
                    <van-row v-if="obj.preparationFlg === 'ON'" class="preparation">
                      準備中
                    </van-row>
                  </van-col>

                  <!-- 已上架和未上架分别有图分15份，无图份24份 -->
                  <van-col
                    :span="obj.image ? 16 : 24"
                    class="list-r"
                    :style="obj.image ? 'width:calc(100% - 120px);' : 'width:calc(100%);'"
                  >
                    <van-row class="food-txt" :class="obj.setExistenceFlg === '0' ? 'height-fixed' : ''">
                      <van-row>
                        <i v-if="obj.pickupFlg === 'ON'" />
                        {{ obj.nameCh }}
                      </van-row>
                      <van-row class="desc">
                        {{ obj.nameJp }}
                      </van-row>
                      <van-row>
                        {{
                          obj.offertimeFrom && obj.offertimeTo ? obj.offertimeFrom + '~' + obj.offertimeTo + 'のみ' : ''
                        }}
                        <b class="appointment">要予約</b>
                      </van-row>
                      <span v-if="obj.setExistenceFlg === '0'" class="price">￥{{ formatAmount(obj.price) }}</span>
                    </van-row>

                    <van-row v-if="obj.setExistenceFlg === '1'" class="set-meal">
                      <van-row class="per-capita-price">
                        <van-row v-for="meal in obj.setNumberList" :key="meal.name" class="per-capita-price-item">
                          <van-row>
                            <span>{{ meal.name }}</span>
                            <span class="price">￥{{ formatAmount(meal.price) }}</span>
                          </van-row>
                        </van-row>
                      </van-row>
                    </van-row>
                  </van-col>
                </van-row>
              </template>
            </van-cell>
          </van-cell-group>
        </van-list>
      </van-tab>
    </van-tabs>
  </div>
</template>
<script>
import { getMenuList } from '@/api/cmb/cmb010'
import { selectShohinBChoiceData } from '@/api/cma/cma100'
import defaultSettings from '@/settings'
export default {
  beforeRouteLeave(to, from, next) {
    let position = window.scrollY // 记录离开页面的位置
    if (position == null) position = 0
    this.$store.commit('setScrollYCommit', position) // 离开路由时把位置存起来
    next()
  },
  name: 'ProductMenu',
  data() {
    return {
      active: '',
      border: '',
      tabActive: 0,
      menuList: [],
      newList: [],
      loading: false,
      finished: true,
      message: '',
      list: [],

      windowWidth: document.documentElement.clientWidth, // 实时屏幕宽度
      windowHeight: document.documentElement.clientHeight // 实时屏幕高度
    }
  },
  computed: {
    imgUrl: function() {
      var imgUrl = `${process.env.VUE_APP_BASE_API}/file/getImage/${defaultSettings.storeNumber}/`
      return imgUrl
    }
  },
  updated() {
    this.$nextTick(function() {
      const position = this.$store.state.setScrollY // 返回页面取出来
      if (position > 0) {
        window.scroll(0, position)
        // this.$store.commit('setScrollYCommit', 0) // 离开路由时把位置存起来
      }
    })
  },
  created() {
    this.getList()
    console.log(this.windowWidth)
    console.log(this.windowHeight)
  },
  methods: {
    getList() {
      this.menuList = []
      getMenuList({ menuNovisibleFlg: '0' }).then(response => {
        if (response.status === 200) {
          this.menuList = response.menuclassList
          // 获取tab
          if (
            sessionStorage.getItem('cmb010Tab') &&
            sessionStorage.getItem('cmb010Tab') !== null &&
            sessionStorage.getItem('cmb010Tab') !== undefined
          ) {
            this.tabActive = parseInt(sessionStorage.getItem('cmb010Tab'))
            this.menuTypeClick(this.menuList[this.tabActive].menuLineNumber)
            // 清除页面缓存
            sessionStorage.removeItem('cmb010Tab')
          } else {
            this.menuTypeClick(this.menuList[0].menuLineNumber)
          }
        } else if (response.status === 601) {
          this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
        } else if (response.status === 602) {
          this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
        }
      })
    },
    menuTypeClick(key) {
      var thisObj = this
      thisObj.newList = []
      selectShohinBChoiceData({ menuLineNumber: key }).then(response => {
        if (response.status === 200) {
          const list = response.shohinList
          let obj = {}
          let setNumberList = []
          list.forEach(item => {
            if (obj.number === item.number) {
              setNumberList.push({ name: item.setName, price: item.price })
            } else {
              obj = item
              setNumberList = []
              setNumberList.push({ name: item.setName, price: item.price })
              item.setNumberList = setNumberList
              thisObj.newList.push(item)
            }
          })
        } else if (response.status === 601) {
          this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
        } else if (response.status === 602) {
          this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
        }
      })
    },
    chooseFood(item, event) {
      // 放入缓存
      sessionStorage.setItem('cmb010Tab', this.tabActive)
      this.$router.push({ path: '/Customer/foodInfo', query: { orderInfo: item.number } })
    },
    onLoad() {},
    getArrayIndex(arr, obj) {
      var i = arr.length
      while (i--) {
        if (arr[i] === obj) {
          return i
        }
      }
      return -1
    }
  }
}
</script>
<style scoped lang="scss">
@import '@/styles/variables.scss';

.menu-list {
  padding: 0;
}

.food-txt {
  height: auto;
  // max-width: 63%;

  .price {
    color: $priceColor;
    font-size: $titleSize;
  }
}

// .set-meal {
//   max-width: 63%;
// }
</style>

<style lang="scss">
@import '@/styles/variables.scss';
// 更改tab自带样式
.menu-list {
  .van-tabs__wrap {
    position: fixed;
    width: 100%;
    z-index: 1;
    line-height: 44px;

    .van-tabs__nav {
      background: linear-gradient(to bottom right, $linearBg1, $linearBg2 100%);
      height: 44px;
      width: 100%;
      display: flex;
      overflow-x: auto;
      overflow-y: hidden;
      white-space: nowrap; // 防止换行
    }

    .van-tab {
      color: $white;

      .van-tab__text {
        min-width: 80px;
        text-align: center;
      }
    }

    .van-tab--active {
      color: $priceColor;
    }

    .van-tabs__line {
      background-color: transparent !important;
    }
  }

  // 内容的padding,预留导航的高度和下面被tabbar遮盖的高度
  .van-tabs__content {
    padding-top: 44px;
    padding-bottom: 84px;
  }

  .van-row--align-center {
    justify-content: start;
  }
}
</style>
