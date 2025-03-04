<template>
  <div>
    <van-row v-if="message !== null && message !== ''" class="error-message">
      {{ message }}
    </van-row>
    <van-row class="product-info">
      <!-- 图文详情 -->
      <van-row class="product-main-info" type="flex" justify="space-between" align="center">
        <!-- 情况1：图文并存 -->
        <van-row v-if="food.image !== ''">
          <van-row gutter="10">
            <van-col span="12" style="width:150px;">
              <van-image :src="imgUrl + food.image" fit="cover" />
            </van-col>
            <van-col span="12" style="width:calc(100% - 150px);">
              <van-row class="food-txt">
                <span>
                  <i v-if="food.pickupFlg == '1'" />
                  {{ food.nameCh }}
                </span>
                <span>
                  {{ food.nameJp }}
                </span>
                <span v-if="food.calorie && food.calorie !== null && food.calorie !== ''">
                  カロリー:{{ formatAmount(food.calorie) + 'kcal' }}
                </span>
                <span v-if="food.offertimeFrom">
                  {{ food.offertimeFrom + '~' + food.offertimeTo + 'のみ' }}
                </span>
                <span v-if="food.price && food.price !== null && food.price !== ''" class="price">
                  {{ food.existenceFlg === '1' ? '' : '￥' + formatAmount(food.price) }}
                </span>
              </van-row>
            </van-col>
          </van-row>
          <!-- 套餐集合 -->
          <van-row v-if="food.courseFlg === '1'" class="menu-list">
            <ul>
              <li v-for="item in courseList" :key="item.id">
                <span class="fl">
                  <b>{{ item.courseOrder }}</b>
                </span>
                <span class="fl">{{ item.courseProductName }}</span>
                <span class="fr">{{ item.courseProductNameJp }}</span>
              </li>
            </ul>
          </van-row>

          <van-row v-if="food.existenceFlg === '1'" class="set-meal">
            <van-row class="per-capita-price">
              <van-row v-for="meal in food.setNumberList" :key="meal.setName" class="per-capita-price-item">
                <van-row>
                  <span>{{ meal.setName }}</span>
                  <span class="price">{{ '￥' + formatAmount(meal.setPrice) }}</span>
                </van-row>
              </van-row>
            </van-row>
          </van-row>
        </van-row>

        <!-- 情况2：无图 -->
        <van-row v-else-if="food.image === ''">
          <van-row gutter="10" class="noimg">
            <van-col span="12">
              <van-row class="food-txt">
                <span>
                  <i v-if="food.pickupFlg == 'ON'" />
                  {{ food.nameCh }}
                </span>
                <span>
                  {{ food.nameJp }}
                </span>
                <span>カロリー:{{ formatAmount(food.calorie) + 'kcal' }}</span>
                <span v-if="food.offertimeFrom">
                  {{ food.offertimeFrom + '~' + food.offertimeTo + 'のみ' }}
                </span>
              </van-row>
            </van-col>
            <van-col span="12">
              <span class="price">{{ food.existenceFlg === '1' ? '' : '￥' + formatAmount(food.price) }}</span>
            </van-col>
          </van-row>

          <!-- 套餐集合 -->
          <van-row v-if="food.courseFlg === '1'" class="menu-list">
            <ul>
              <li v-for="item in courseList" :key="item.id">
                <span class="fl">
                  <b>{{ item.courseOrder }}</b>
                </span>
                <span class="fl">{{ item.courseProductName }}</span>
                <span class="fr">{{ item.courseProductNameJp }}</span>
              </li>
            </ul>
          </van-row>

          <van-row v-if="food.existenceFlg === '1'" class="set-meal">
            <van-row class="per-capita-price">
              <van-row v-for="meal in food.setNumberList" :key="meal.setName" class="per-capita-price-item">
                <van-row>
                  <span>{{ meal.setName }}</span>
                  <span class="price">{{ '￥' + formatAmount(meal.setPrice) }}</span>
                </van-row>
              </van-row>
            </van-row>
          </van-row>
        </van-row>
      </van-row>

      <!-- 商品介绍 -->
      <van-row class="detail-info">
        <van-row class="detail-info-header">
          商品紹介
        </van-row>
        <van-row class="detail-info-body" />
      </van-row>

      <!-- 过敏信息 -->
      <van-row v-if="showAllergies" class="allergies-info">
        <van-row v-for="index of categoryList.length / 4" :key="index">
          <ul>
            <li v-for="(value, key) in categoryList.slice(index * 4 - 4, index * 4)" :key="key">
              <span>{{ value.categoryKbnName }}</span>
            </li>
          </ul>
          <ul>
            <li v-for="(value, key) in categoryList.slice(index * 4 - 4, index * 4)" :key="key">
              <span v-if="value.allergiesFlg !== '-'">{{ value.allergiesFlg === '1' ? '〇' : '×' }}</span>
              <span v-else>{{ value.allergiesFlg }}</span>
            </li>
          </ul>
        </van-row>

        <!-- 最后一行字 -->
        <van-row class="tips" />
      </van-row>
    </van-row>
  </div>
</template>
<script>
import { getFoodInfo } from '@/api/cmb/cmb020'
import { searchKuBun } from '@/api/common'
import defaultSettings from '@/settings'
export default {
  name: 'FoodInfo',
  data() {
    return {
      courseList: [],
      newList: [],
      showAllergies: false,
      active: '',
      border: '',
      index: '',
      food: {},
      message: '',
      categoryList: [],
      allergiesList: [],
      menuList: []

      // 图文详情，文字+菜品，文字+套餐三种情况
    }
  },
  computed: {
    imgUrl: function() {
      var imgUrl = `${process.env.VUE_APP_BASE_API}/file/getImage/${defaultSettings.storeNumber}/`
      return imgUrl
    }
  },
  created() {},
  mounted() {
    if (this.$route.query.orderInfo) {
      var orderInfo = this.$route.query.orderInfo
      var param = { productNumber: orderInfo }
      this.getFoodInfo(param)
    }
  },
  methods: {
    getFoodInfo(param) {
      const thisObj = this

      getFoodInfo(param).then(response => {
        if (response.status === 200) {
          thisObj.courseList = response.courseList
          const list = response.shohinDetailsList
          thisObj.food = list[0]
          document.getElementsByClassName('detail-info-body')[0].innerHTML = thisObj.food.introduction
          if (response.storeList[0].allergiesDisplayFlg === '1') {
            thisObj.allergiesList = response.allergiesList

            searchKuBun({ categoryClassNumber: '002' }).then(response => {
              for (var i = 0; i < response.categoryList.length; i++) {
                for (var j = 0; j < thisObj.allergiesList.length; j++) {
                  if (response.categoryList[i].categoryKbn === thisObj.allergiesList[j].allergiesKbn) {
                    response.categoryList[i].allergiesFlg = thisObj.allergiesList[j].allergiesFlg
                    break
                  }
                }
              }

              // 处理过敏集合
              if (response.categoryList.length % 4 !== 0) {
                for (let i = 0; i < response.categoryList.length % 4; i++) {
                  response.categoryList.push({ categoryKbnName: '-', allergiesFlg: '-' })
                }
              }

              thisObj.categoryList = response.categoryList
            })

            thisObj.showAllergies = true
          }
        } else if (response.status === 601) {
          this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
        } else if (response.status === 602) {
          this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
        }
      })
    }
  }
}
</script>
<style scoped lang="scss">
@import '@/styles/variables.scss';
.product-info {
  padding: 20px;

  // 上部分，图文详情
  .product-main-info {
    > .van-row {
      width: 100%;
      min-height: 50px;
    }

    .van-image {
      width: 140px;
      height: 140px;
    }

    // 修改公共价格位置
    .food-txt {
      line-height: 24px;
      min-width: 50%;
      position: relative;
      height: 140px;

      .price {
        left: 0;
        bottom: 0;
        text-align: left;
      }

      span {
        display: block;
      }
    }

    .noimg {
      .food-txt {
        height: auto;
      }

      .price {
        float: right;
        color: $priceColor;
      }
    }

    .menu-list {
      margin-top: 20px;

      li {
        width: 100%;
        height: 40px;
        line-height: 40px;
        background-color: rgba($color: $white, $alpha: 0.1);
        margin-bottom: 5px;
        padding: 0 20px;
        box-sizing: border-box;

        b {
          font-weight: normal;
          color: rgba($color: $white, $alpha: 0.6);
          margin-right: 20px;
        }
      }
    }

    .set-meal {
      margin-top: 20px;
    }
  }

  // 商品介绍
  .detail-info {
    border: 2px solid $footerBorderTopLinear2;
    border-radius: 5px;
    margin-top: 20px;

    .detail-info-header {
      padding: 10px;
      border-bottom: 2px solid $footerBorderTopLinear2;
    }

    .detail-info-body {
      min-height: 140px;
      padding: 10px;
      height: 180px;
      overflow-y: auto;
    }
  }

  // 过敏信息
  .allergies-info {
    margin-top: 20px;
    border-top: 1px solid $footerBorderTopLinear2;

    ul:first-child {
      span {
        opacity: 0.6;
      }
    }

    ul {
      overflow: hidden;
      border: 1px solid $footerBorderTopLinear2;
      border-top: 0;

      li {
        width: 25%;
        float: left;
        text-align: center;
        height: 40px;
        line-height: 40px;
        border-right: 1px solid $footerBorderTopLinear2;
        box-sizing: border-box;
      }

      li:last-child {
        border-right: 0;
      }
    }
  }

  // 最后一行提示文字
  .tips {
    margin: 20px 0 120px;
    opacity: 0.6;
  }
}

// .smail-font {
//   font-size: 12px;
//   overflow: hidden;
//   width: 160px;
//   height: 20px;
//   line-height: 20px;
// }

// .body {
//   margin-top: 10px;
//   margin-bottom: 45px;
// }
// .detailed-Info {
//   height: 220px;
//   border-radius: 7px;
//   border: 2px solid mediumturquoise;
// }
// .detailed-Info-header {
//   border-bottom: 2px solid mediumturquoise;
// }
// .detailed-Info-body {
//   height: 185px;
//   width: 100%;
//   overflow-y: auto;
//   padding: 2px;
//   box-sizing: border-box;
// }
</style>
<style lang="scss">
// .van-row {
//   padding: 5px;
// }
// .MyTable {
//   border: 1px solid gold;
// }
</style>
