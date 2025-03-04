<template>
  <div>
    <van-sticky :offset-top="77">
      <van-row class="txt-desc">
        注文可能数を変更する商品分類を選択してください
      </van-row>
    </van-sticky>

    <van-row class="prolist">
      <van-cell v-for="item in dataList" :key="item.categoryKbn" is-link @click="clickName(item)">
        <span>{{ item.categoryKbnName }}</span>
      </van-cell>
    </van-row>
  </div>
</template>

<script>
import { searchKuBun } from '@/api/common'
export default {
  name: 'Cma190',
  data() {
    return {
      dataList: []
    }
  },
  created() {
    this.init()
  },
  methods: {
    // 检索
    init() {
      const query = { categoryClassNumber: '031' }
      searchKuBun(query).then(response => {
        if (response.status === 200) {
          this.dataList = response.categoryList
        }
      })
    },
    clickName(item) {
      this.$router.push({
        path: '/Employee/changeOfOrderableQuantity',
        query: { code: item.categoryKbn, name: item.categoryKbnName }
      })
    }
  }
}
</script>

<style lang="scss" scoped>
@import '@/styles/variables.scss';

.prolist {
  margin-bottom: 130px;
}

.txt-desc {
  font-size: $moreSmallSize;
}
</style>
