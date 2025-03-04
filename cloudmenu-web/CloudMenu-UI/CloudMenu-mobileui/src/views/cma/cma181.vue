<template>
  <div>
    <van-row v-if="showError" class="error-message">
      <van-col>{{ message }}</van-col>
    </van-row>
    <van-sticky :offset-top="77">
      <van-row class="txt-desc">{{ query.itemKbnName }}分類</van-row>
    </van-sticky>
    <div>
      <van-cell
        v-for="(item, index) in list"
        :key="index"
        :title="item.categoryKbnName"
        @click="changeColor($event, item)"
      />
    </div>
  </div>
</template>

<script>
import { searchKuBun } from '@/api/common'
export default {
  name: 'SeatSelection',
  data() {
    return {
      showError: false,
      query: {
        itemKbn: '',
        itemKbnName: '',
        storingDate: '',
        categoryKbn: '',
        categoryKbnName: ''
      },
      list: [],
      element: {
        style: {
          backgroundColor: null
        }
      }
    }
  },
  created() {
    // 获取上一个画面数据
    if (sessionStorage.getItem('180Query') && sessionStorage.getItem('180Query') !== null && sessionStorage.getItem('180Query') !== undefined) {
      const query = JSON.parse(sessionStorage.getItem('180Query'))
      this.query.itemKbn = query.itemKbn
      this.query.itemKbnName = query.itemKbnName
      this.query.storingDate = query.storingDate
    }
    // 初始化
    this.init()
  },
  methods: {
    init() {
      searchKuBun({ categoryClassNumber: this.query.itemKbn }).then(response => {
        if (response.status === 200) {
          this.list = response.categoryList
        } else if (response.status === 601) {
          this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
        } else if (response.status === 602) {
          this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
        }
      })
    },

    changeColor($event, item) {
      if (this.element.style) {
        this.element.style.backgroundColor = ''
      }
      // 追加商品分类
      if (item.categoryKbn && item.categoryKbnName) {
        this.query.categoryKbn = item.categoryKbn
        this.query.categoryKbnName = item.categoryKbnName
      }

      this.element = $event.currentTarget
      $event.currentTarget.style.backgroundColor = '#5e5b64'
      // 重新赋值
      sessionStorage.setItem('180Query', JSON.stringify(this.query))
      // 跳转画面
      this.$router.push({ path: '/Employee/SelectionOfIncomingoods' })
    }
  }
}
</script>

<style lang="scss" scoped></style>
