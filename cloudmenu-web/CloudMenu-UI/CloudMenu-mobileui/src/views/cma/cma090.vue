<template>
  <div>
    <van-row v-if="message !== null && message !== ''" class="error-message">
      {{ message }}
    </van-row>
    <van-sticky :offset-top="77">
      <van-row class="search">
        <van-search
          id="valueInput"
          v-model="queryParam.menuOrderNumber"
          v-Alphabet
          autofocus="true"
          tabindex="10"
          maxlength="10"
          placeholder="番号から探す"
        />
        <i class="btn-search" tabindex="10" @click="searchFoodNo">
          <img src="@/assets/images/search.png" alt="" />
        </i>
      </van-row>
      <van-row class="txt-desc">
        分類から探す
      </van-row>
    </van-sticky>
    <van-row class="cell-list">
      <van-cell
        v-for="(item, idx) in list"
        :key="idx"
        :border="border"
        :title="item.menuClassName"
        @click="chooseMenuType(item.menuLineNumber)"
      />
    </van-row>

    <!-- <div class="footer"></div> -->
  </div>
</template>
<script>
import { getList } from '@/api/cma/cma090'
import Vue from 'vue'
// 自定义指令 ,只能输入字母和数字
Vue.directive('Alphabet', {
  inserted: function(el) {
    const input = el.querySelector('#valueInput')
    input.onkeyup = function(e) {
      input.value = input.value.replace(/[^A-Za-z0-9]/g, '')
    }
    input.onblur = function(e) {
      input.value = input.value.replace(/[^A-Za-z0-9]/g, '')
    }
  }
})
export default {
  name: 'CommoditySearchMethod',
  data() {
    return {
      index: '',
      list: [],
      border: true,
      message: '',
      showError: false,
      receptionList: [],
      queryParam: {
        receptionKbn: '',
        mainSeat: {},
        menuOrderNumber: '',
        menuLineNumber: ''
      }
    }
  },
  created() {
    if (
      sessionStorage.getItem('seatInfo') &&
      sessionStorage.getItem('seatInfo') !== null &&
      sessionStorage.getItem('seatInfo') !== undefined
    ) {
      const queryParam = JSON.parse(sessionStorage.getItem('seatInfo'))
      this.queryParam = queryParam
    }
    this.getList()
  },
  methods: {
    getList() {
      this.message = ''
      getList({ menuNovisibleFlg: '1' }).then(response => {
        if (response.status === 200) {
          this.list = response.menuclassList
        } else if (response.status === 601) {
          this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
        } else if (response.status === 602) {
          this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
        }
      })
    },
    searchFoodNo() {
      this.message = ''
      this.queryParam.menuLineNumber = 0
      if (!this.queryParam.menuOrderNumber || this.queryParam.menuOrderNumber === '') {
        this.$msgUtil.messageNew('E_00020', '商品メニュー注文番号', this)
        return
      }
      // 放入缓存
      sessionStorage.setItem('seatInfo', JSON.stringify(this.queryParam))
      this.$router.push({ path: '/Employee/CommodityOrder' })
    },
    chooseMenuType(typeNo) {
      this.queryParam.menuOrderNumber = ''
      this.queryParam.menuLineNumber = typeNo
      // 放入缓存
      sessionStorage.setItem('seatInfo', JSON.stringify(this.queryParam))
      this.$router.push({ path: '/Employee/CommodityOrder' })
    }
  }
}
</script>

<style scoped lang="scss">
@import '@/styles/variables.scss';
.body {
  height: 750px;
}

.txt-desc {
  padding-left: 25px;
}

.cell-list {
  padding-bottom: 100px;
}
</style>

<style lang="scss">
@import '@/styles/variables.scss';

.van-tabbar--fixed {
  border-top: 0;
}
</style>
