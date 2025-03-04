<template>
  <div>
    <van-row v-if="message !== null && message !== ''" class="error-message">
      <van-col>{{ message }}</van-col>
    </van-row>
    <van-row class="start-warehousing">
      <!-- 单选 -->
      <van-row class="top">
        <van-radio-group v-model="itemKbn" direction="horizontal">
          <van-radio v-for="(item, index) in radioList" :key="index" :name="item.id" :label="item.label" class="radio">
            {{ item.label }}
          </van-radio>
        </van-radio-group>
      </van-row>
      <!-- 选日期检索 -->
      <van-row class="search search-reset" @click="showpopup">
        <van-search disabled tabindex="10" maxlength="10" placeholder="入庫日" :value="storingDate" />
        <label for="" class="error-message position-error">{{ error }}</label>
        <i class="btn-search" tabindex="10" @click.stop.prevent="getList()">
          <img src="@/assets/images/search.png" alt="">
        </i>
      </van-row>
      <van-popup v-model="isShow" position="bottom">
        <van-datetime-picker
          v-model="currentDate"
          type="date"
          :min-date="minDate"
          :max-date="maxDate"
          confirm-button-text="確認"
          cancel-button-text="キャンセル"
          @confirm="sureDate"
          @cancel="cancelDate"
        />
      </van-popup>

      <!-- 加号按钮 -->
      <van-row v-if="enableAddButton" class="add btn-linear" @click="toWarehousing()">
        <van-icon name="plus" />
      </van-row>
    </van-row>

    <!-- 检索结果 -->
    <van-row v-if="searchList.length > 0" class="warehouseing-search-result">
      <van-cell v-for="(item, index) in searchList" :key="item.targetNumber">
        <ul class="data-item">
          <li style="width:10%;">{{ index + 1 }}</li>
          <li style="width:20%;">{{ item.categoryName }}</li>
          <li style="width:50%;">{{ item.targetName }}</li>
          <li style="width:10%;">{{ item.storingQuantity }}{{ item.unitName }}</li>
        </ul>
        <!-- <van-grid direction="horizontal" :column-num="2">
          <van-grid-item :text="index + 1" />
          <van-grid-item :text="item.categoryName" />
          <van-grid-item :text="item.targetName" />
          <van-grid-item :text="item.storingQuantity + item.unitName" />
        </van-grid> -->
        <!--
        <span style="width:20%;">{{ index + 1 }}</span>
        <span style="width:60%;">{{ item.categoryName }}</span>
        <span style="width:10%;">{{ item.targetName }}</span>
        <span style="width:10%;">{{ item.storingQuantity }}{{ item.unitName }}</span> -->
      </van-cell>
    </van-row>

    <van-row v-else class="search-result-nomsg">
      なし
    </van-row>
  </div>
</template>

<script>
import { searchNyukoList } from '@/api/cma/cma180'
import { formatter } from '@/utils/formatter.js'
export default {
  name: 'Cma180',
  data() {
    return {
      itemKbn: '031',
      storingDate: '',
      radioList: [
        { id: '031', label: '商品' },
        { id: '032', label: '原材料' },
        { id: '033', label: '備品' }
      ],
      isShow: false,
      minDate: new Date(1990, 1, 1),
      maxDate: new Date(2030, 1, 1),
      currentDate: new Date(),
      nowDate: '',
      error: '', // 输入框错误提示
      enableAddButton: true,
      searchList: [],
      loading: false, // 是否正在加载中，防止多次触发
      finished: false, // 是否结束
      paramsList: {
        orgId: '',
        pageIndex: 0,
        pageSize: 10
      },
      showError: false,
      message: null
    }
  },
  created() {
    // 获取上一个画面数据
    if (
      sessionStorage.getItem('180Query') &&
      sessionStorage.getItem('180Query') !== null &&
      sessionStorage.getItem('180Query') !== undefined
    ) {
      const query = JSON.parse(sessionStorage.getItem('180Query'))
      this.itemKbn = query.itemKbn
      this.storingDate = query.storingDate

      this.getList()
    }
    // 清除页面缓存
    sessionStorage.removeItem('180Query')
  },
  methods: {
    // 检索一览数据
    getList() {
      this.message = null
      // 判断当前入库时间

      this.nowDate = formatter.formatDateSelf(new Date(), 'yyyy/MM/dd')

      var date = this.storingDate
      if (date === '') {
        this.$msgUtil.messageNew('E_00020', '入庫日', this, 'error')
        this.searchList = []
        return false
      } else if (Date.parse(date) > Date.parse(this.nowDate)) {
        // 选择日期是未来日
        this.$msgUtil.messageNew('E_00130', '', this)
        this.searchList = []
        return false
      }
      const query = { itemKbn: this.itemKbn, storingDate: this.storingDate }
      this.searchNyukoList(query)
    },

    // 判断是否检索出来数据
    searchNyukoList(query) {
      searchNyukoList(query).then(response => {
        if (response.status === 200) {
          this.searchList = response.searchList
          if (this.searchList.length === 0) {
            //
          } else {
            if (
              this.searchList.length === 0 ||
              this.paramsList.pageIndex * this.paramsList.pageSize > this.searchList.length
            ) {
              this.finished = true // 所有的数据已经全部加载完了
            } else {
              this.finished = false
            }
          }
        } else if (response.status === 601) {
          this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
        } else if (response.status === 602) {
          this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
        }
      })
    },

    // 上架
    toWarehousing() {
      this.message = null
      // 判断当前入库时间

      this.nowDate = formatter.formatDateSelf(new Date(), 'yyyy/MM/dd')

      var date = this.storingDate
      if (date === '') {
        this.$msgUtil.messageNew('E_00020', '入庫日', this, 'error')
        this.searchList = []
        return false
      } else if (Date.parse(date) > Date.parse(this.nowDate)) {
        // 选择日期是未来日
        this.$msgUtil.messageNew('E_00130', '', this)
        this.searchList = []
        return false
      }

      let itemKbnName = ''
      for (let i = 0; i < this.radioList.length; i++) {
        if (this.itemKbn === this.radioList[i].id) {
          itemKbnName = this.radioList[i].label
          break
        }
      }

      const query = { itemKbn: this.itemKbn, storingDate: this.storingDate, itemKbnName: itemKbnName, startware: true }
      // 放入缓存
      sessionStorage.setItem('180Query', JSON.stringify(query))

      this.$router.push({ path: '/Employee/IncomingGoodsClassifySelect' })
    },
    // ----日期选择-------------------------------------------------
    // 日期弹出的显示与隐藏
    showpopup() {
      if (!this.isShow) {
        this.isShow = true
      } else {
        this.isShow = false
      }
    },

    // 确定日期
    sureDate(val) {
      this.storingDate = formatter.formatDateSelf(val, 'yyyy/MM/dd')
      var nowData = formatter.formatDateSelf(new Date(), 'yyyy/MM/dd')
      if (nowData > this.storingDate) {
        this.enableAddButton = false
      } else {
        this.enableAddButton = true
      }

      this.isShow = false
      // 选择日期，清空报红信息和检索为0的信息
      this.error = ''
    },

    // 取消
    cancelDate() {
      this.isShow = false
    }
    // ----日期选择结束-------------------------------------------------
  }
}
</script>

<style lang="scss" scoped>
@import '@/styles/variables.scss';

.start-warehousing {
  padding: 0 24px;
  height: 100%;

  .warehouseing-search-result {
    text-align: center;
    opacity: 0.7;
  }

  .add {
    width: 56px;
    height: 56px;
    border-radius: 50%;
    display: flex;
    justify-content: center;
    align-items: center;
    position: fixed;
    bottom: 20px;
    right: 24px;
    z-index: 1;

    .van-icon-plus {
      font-size: 28px;
    }
  }

  .error-message {
    padding-left: 0;
  }
}

.search-result-nomsg {
  text-align: center;
}

.data-item li {
  float: left;
  margin-right: 2px;
}
</style>

<style lang="scss">
@import '@/styles/variables.scss';

.van-cell {
  padding: 12px 24px;
}
.van-cell__value {
  color: $white;

  span {
    margin-right: 20px;
  }

  :first-child {
    margin-right: 30px;
  }
}
</style>
