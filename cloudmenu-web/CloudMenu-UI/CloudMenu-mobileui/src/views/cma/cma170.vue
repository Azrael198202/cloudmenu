<template>
  <div ref="sellineName" @click="closeSel">
    <van-row v-if="message !== '' && message !== null" class="error-message">
      <van-col>{{ message }}</van-col>
    </van-row>
    <van-row class="start-warehousing">
      <!-- 选日期检索 -->
      <van-row class="search search-reset" @click.stop="isDateShow = true">
        <van-search tabindex="10" readonly maxlength="10" placeholder="棚卸日" :value="inventoryDate" />
        <label v-if="dateError !== null && dateError !== ''" class="error-message position-error">
          {{ dateError }}
        </label>
      </van-row>

      <!-- 单选 -->
      <van-row class="top">
        <van-radio-group v-model="itemKbn" direction="horizontal" @change="itemKbnChange">
          <van-radio v-for="(item, index) in itemKbnList" :key="index" :name="item.id" :aa="index">
            {{ item.name }}
          </van-radio>
        </van-radio-group>
      </van-row>

      <!-- 下拉 -->
      <van-row class="dorpdown">
        <van-col span="16">
          <van-row class="select-down">
            <van-row class="select-input">
              <input v-model="categoryName" type="text" @click.stop="searchInfo">
              <i class="fa fa-caret-down" aria-hidden="true" />
            </van-row>

            <ul ref="selectList">
              <li v-for="(item, index) in items" :key="index + 'x'" @click.stop="selectCurrent(item.value, $event)">
                <span>{{ item.text }}</span>
              </li>
            </ul>
          </van-row>
        </van-col>
        <van-col span="8">
          <van-button type="default" @click.stop="unloading">棚卸</van-button>
        </van-col>
        <!-- TODO 错误信息位置 -->
        <label v-if="categoryError !== null && categoryError !== ''" class="error-message position-drop">
          {{ categoryError }}
        </label>
      </van-row>

      <div class="list-card">
        <ul v-for="(item, index) in searchList" :key="item.No" ref="listItem" class="listitem" :aa="'listItem' + index">
          <li>
            <van-row>
              <van-col span="13">
                <span style="color:#c1c0c2">No</span>
              </van-col>
              <van-col span="11">
                <span>{{ item.targetNumber }}</span>
              </van-col>
            </van-row>
          </li>
          <li>
            <van-row>
              <van-col span="13">
                <span style="color:#c1c0c2">名称</span>
              </van-col>
              <van-col span="11" class="oneline-overflow">
                <span>{{ item.targetName }}</span>
              </van-col>
            </van-row>
          </li>
          <li>
            <van-row>
              <van-col span="13">
                <span style="color:#c1c0c2">前棚卸日</span>
              </van-col>
              <van-col span="11">
                <span>{{ item.lastDate }}</span>
              </van-col>
            </van-row>
          </li>
          <li>
            <van-row>
              <van-col span="13">
                <span style="color:#c1c0c2">前確定數</span>
              </van-col>
              <van-col span="11">
                <span>{{ item.lastQuantity }}</span>
              </van-col>
            </van-row>
          </li>
          <li>
            <van-row>
              <van-col span="13">
                <span style="color:#c1c0c2">入</span>
              </van-col>
              <van-col span="11">
                <span>{{ item.storingQuantity }}</span>
              </van-col>
            </van-row>
          </li>
          <li>
            <van-row>
              <van-col span="13">
                <span style="color:#c1c0c2">消</span>
              </van-col>
              <van-col span="11">
                <span>{{ item.consumptionQuantity }}</span>
              </van-col>
            </van-row>
          </li>
          <li>
            <van-row>
              <van-col span="13">
                <span style="color:#c1c0c2">論理在庫數</span>
              </van-col>
              <van-col span="11">
                <span>{{ item.stockQuantity }}</span>
              </van-col>
            </van-row>
          </li>
          <li>
            <van-row>
              <van-col span="13">
                <span style="color:#c1c0c2">棚卸數</span>
              </van-col>
              <van-col span="11">
                <input v-model="item.inventoryQuantity" type="number">
              </van-col>
            </van-row>
          </li>
          <li>
            <van-row>
              <van-col span="13">
                <span style="color:#c1c0c2">差異</span>
              </van-col>
              <van-col span="11">
                <span>{{ difference(item, index) }}</span>
              </van-col>
            </van-row>
          </li>
          <li><span @click.stop="inputDetailDialog(item, index)">内訳入力</span></li>
        </ul>
      </div>
    </van-row>

    <van-button class="btn-linear" type="primary" :disabled="searchList.length === 0" @click.stop="submit">
      棚卸確定
    </van-button>

    <van-popup v-model="isDateShow" position="bottom">
      <van-datetime-picker
        v-model="currentDate"
        type="date"
        :min-date="minDate"
        :max-date="maxDate"
        :formatter="formatter"
        confirm-button-text="確認"
        cancel-button-text="キャンセル"
        @confirm="sureDate"
        @cancel="isDateShow = false"
      />
    </van-popup>

    <van-popup v-model="showDialog" class="order-remarks">
      <van-row class="title">
        内訳入力
      </van-row>
      <van-row class="top-info">
        <span>No {{ tempItem.targetNumber }}</span>
        <span>{{ tempItem.targetName }}</span>
        <span>差異：{{ tempItem.difference }}</span>
      </van-row>
      <van-row class="input-box">
        <van-row>
          <span>入庫:</span>
          <input v-model="tempItem.inNum" type="number">
        </van-row>
        <van-row>
          <span>出庫:</span>
          <input v-model="tempItem.outNum" type="number">
        </van-row>
        <van-row>
          <span>使用:</span>
          <input v-model="tempItem.useNum" type="number">
        </van-row>
        <van-row>
          <span>破棄:</span>
          <input v-model="tempItem.loseNum" type="number">
        </van-row>
        <van-row>
          <span>破損:</span>
          <input v-model="tempItem.wornNum" type="number">
        </van-row>
      </van-row>

      <van-row class="row-btn">
        <van-button type="default" class="cancel fl" @click.stop="inputClick()">入力</van-button>
      </van-row>
    </van-popup>
  </div>
</template>

<script>
import { searchTana, saveTana } from '@/api/cma/cma170'
import { searchKuBun } from '@/api/common'
import $ from 'jquery'
export default {
  name: 'Cma170',
  data() {
    return {
      message: null, // 顶部错误
      inventoryDate: null, // 棚卸日
      isDateShow: false,
      dateError: null,
      itemKbn: '031', // 品目区分
      itemKbnList: [
        { id: '031', name: '商品' },
        { id: '032', name: '原材料' },
        { id: '033', name: '備考' }
      ],
      categoryKbn: null, // 分類区分
      categoryName: null, // 分類区分
      categoryList: [], // 分類区分列表
      categoryError: null,
      searchList: [],
      // 控制棚卸確定活性
      submitFlag: false,
      // 存储当前所选行数据
      tempItem: {
        tempItemIndex: null,
        targetName: null,
        targetNumber: 0,
        difference: null,
        inNum: null,
        outNum: null,
        useNum: null,
        loseNum: null,
        wornNum: null,
        // 入出庫・消費フラグ 0:入出庫1:消費
        targetFlg: null,
        // 区分
        breakdownKbn: null,
        // 内訳数
        breakdownQuantity: 0
      },
      minDate: new Date(1990, 1, 1),
      maxDate: new Date(9999, 1, 1),
      currentDate: new Date(),
      // 下拉
      showDialog: false, // 弹框
      // 输入库存数
      show: false
    }
  },
  computed: {
    // 计算差异值=输入库存数-理论库存数
    difference() {
      return function(item, index) {
        item.inputDetailFlag = false
        if (!item.inventoryQuantity) {
          return ''
        }
        item.difference = item.inventoryQuantity - item.stockQuantity
        if (item.difference !== 0) {
          item.inputDetailFlag = true
        }

        const listItem = this.$refs.listItem[index]
        listItem.style.color = '#ffffff'

        return item.difference
      }
    },
    // 过滤方法
    items: function() {
      const _search = this.categoryName
      if (_search) {
        // 不区分大小写处理
        const reg = new RegExp(_search, 'ig')
        // es6 filter过滤匹配，有则返回当前，无则返回所有
        return this.categoryList.filter(function(e) {
          // 匹配所有字段
          return Object.keys(e).some(function(key) {
            return e[key].match(reg)
          })
        })
      }
      return this.categoryList
    }
  },
  mounted() {
    // 点击其他区域关闭下拉菜单
    document.addEventListener('click', e => {
      if (!this.$el.contains(e.target)) {
        this.show = false
      }
    })
  },
  created() {
    // 设置当前日期
    this.sureDate(this.currentDate)
    // 根据区分分类获取下拉框的值
    this.itemKbnChange()
  },
  methods: {
    // 确定日期
    sureDate(val) {
      const params = this.formatDate(val)
      this.inventoryDate = params.year + '/' + params.month + '/' + params.day
      this.isDateShow = false
      // 选择日期，清空报红信息
      this.dateError = null
    },
    // 根据区分分类获取下拉框的值
    itemKbnChange() {
      this.categoryKbn = null
      this.categoryName = null
      this.categoryList = []
      const data = { categoryClassNumber: this.itemKbn }
      searchKuBun(data).then(response => {
        const categoryList = response.categoryList
        for (let i = 0; i < categoryList.length; i++) {
          const item = {}
          item.text = categoryList[i].categoryKbnName
          item.value = categoryList[i].categoryKbn
          this.categoryList.push(item)
        }
      })
    },
    // 棚卸按钮押下
    unloading() {
      this.message = null
      this.dateError = null
      this.categoryError = null
      this.searchList = []
      if (this.inventoryDate === null || this.inventoryDate === '') {
        this.$msgUtil.messageNew('E_00020', '棚卸日', this, 'dateError')
      }
      if (this.categoryKbn === null || this.categoryKbn === '') {
        this.$msgUtil.messageNew('E_00020', '分類区分', this, 'categoryError')
      }
      const nowDate = this.formatDate(new Date())
      const nowDate1 = nowDate.year + '/' + nowDate.month + '/' + nowDate.day
      if (this.inventoryDate > nowDate1) {
        this.$msgUtil.messageNew('E_00130', '', this)
      }
      if (
        (this.dateError !== null && this.dateError !== '') ||
        (this.categoryError !== null && this.categoryError !== '') ||
        (this.message !== null && this.message !== '')
      ) {
        return
      }
      this.$msgUtil
        .messageBox('I_00050')
        .then(() => {
          const param = { inventoryDate: this.inventoryDate, itemKbn: this.itemKbn, categoryKbn: this.categoryKbn }
          searchTana(param).then(response => {
            if (response.status === 200) {
              this.submitFlag = true
              for (let i = 0; i < response.searchList.length; i++) {
                const el = response.searchList[i]
                // 控制内訳入力活性非活性
                el.difference = null
                el.inputDetailFlag = false
                el.breakdownQuantity = 0
                el.inventoryQuantity = null
                this.searchList.push(el)
              }
            } else if (response.status === 601) {
              this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
            } else if (response.status === 602) {
              this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
            }
          })
        })
        .catch(() => {})
    },

    // 弹窗入力按钮点击,内訳入力_入力ボタン押下
    inputClick() {
      const breakdownQuantity =
        parseInt(this.tempItem.inNum === null ? 0 : this.tempItem.inNum) -
        parseInt(this.tempItem.outNum === null ? 0 : this.tempItem.outNum) -
        parseInt(this.tempItem.useNum === null ? 0 : this.tempItem.useNum) -
        parseInt(this.tempItem.loseNum === null ? 0 : this.tempItem.loseNum) -
        parseInt(this.tempItem.wornNum === null ? 0 : this.tempItem.wornNum)
      if (this.tempItem.difference !== breakdownQuantity) {
        this.$msgUtil
          .messageBox('W_00060', '')
          .then(() => {
            this.searchList[this.tempItem.tempItemIndex].breakdownQuantity = breakdownQuantity
            this.showDialog = false
          })
          .catch(() => {
            this.showDialog = false
            return
          })
      } else {
        this.searchList[this.tempItem.tempItemIndex].breakdownQuantity = breakdownQuantity
        this.showDialog = false
      }
    },
    // 棚卸確定
    submit() {
      if (!this.submitFlag) {
        // 棚卸確定非活性时
        return
      }
      let messageFlag = false
      // 请求参数棚卸一覧
      const tanaList = []
      // 请求参数内訳一覧
      const uchiwakeList = []
      for (let i = 0; i < this.searchList.length; i++) {
        const tanaEl = {
          targetNumber: this.searchList[i].targetNumber,
          targetName: this.searchList[i].targetName,
          inventoryQuantity: this.searchList[i].inventoryQuantity
        }
        const uchiwakeEl = {
          targetNumber: this.searchList[i].targetNumber,
          targetName: this.searchList[i].targetName,
          targetFlg: null,
          breakdownKbn: null,
          breakdownQuantity: this.searchList[i].breakdownQuantity
        }
        if (this.searchList[i].difference !== 0) {
          messageFlag = true
        }
        if (this.searchList[i].breakdownQuantity !== this.searchList[i].difference) {
          this.$msgUtil.messageNew('E_00150', this.searchList[i].targetNumber, this)
          const listItem = this.$refs.listItem[i]
          listItem.style.color = '#fd5555'

          $('.listitem')
            .eq(i)
            .addClass('listitem activered')

          return
        }
        tanaList.push(tanaEl)
        uchiwakeList.push(uchiwakeEl)
      }
      if (!messageFlag) {
        // 差异数全部为空时,退出处理，爆出消息
        this.$msgUtil.messageNew('E_00140', '', this)
        return
      }
      this.message = null
      this.dateError = null
      this.categoryError = null
      // check通过之后后续处理
      this.$msgUtil
        .messageBox('I_00060')
        .then(() => {
          const param = {
            inventoryDate: this.inventoryDate,
            itemKbn: this.itemKbn,
            categoryKbn: this.categoryKbn,
            tanaList: tanaList,
            uchiwakeList: uchiwakeList
          }
          // TODO 参数
          saveTana(param).then(response => {
            if (response.status === 200) {
              this.$msgUtil.messageNew('I_00070', '棚卸確定')
              this.searchList = []
            } else if (response.status === 601) {
              this.$msgUtil.messageNew(response.msgCode, '', this)
            } else if (response.status === 602) {
              this.$msgUtil.messageNew(response.msgCode, '', this)
            }
          })
        })
        .catch(() => {
          return
        })
    },
    // 初始化当前日期
    formatter(type, value) {
      if (type === 'year') {
        return value
      } else if (type === 'month') {
        return value
      } else if (type === 'day') {
        return value
      }
      return value
    },
    // 格式化日期方法
    formatDate(val) {
      const year = val.getFullYear() // 年
      let month = val.getMonth() + 1 // 月
      let day = val.getDate() // 日
      if (month >= 1 && month <= 9) {
        month = '0' + month
      }
      if (day >= 1 && day <= 9) {
        day = '0' + day
      }
      return { year: year, month: month, day: day }
    },
    // ----日期选择结束-------------------------------------------------
    inputDetailDialog(item, index) {
      if (item.inputDetailFlag) {
        // 活性时的操作,打开内訳入力弹框
        this.tempItem.targetName = item.targetName
        this.tempItem.targetNumber = item.targetNumber
        this.tempItem.difference = item.difference
        this.tempItem.tempItemIndex = index
        this.tempItem.inNum = null
        this.tempItem.outNum = null
        this.tempItem.useNum = null
        this.tempItem.loseNum = null
        this.tempItem.wornNum = null
        this.showDialog = true
      }
    },

    // 选中当前检索项
    selectCurrent(value, event) {
      this.categoryName = event.currentTarget.innerText
      this.categoryKbn = value
      this.$refs.selectList.style.display = 'none'
    },

    // 输入框输入事件
    searchInfo() {
      this.$refs.selectList.style.display = 'block'
    },

    closeSel(event) {
      const currentCli = this.$refs.sellineName
      if (currentCli) {
        // 点击到了id为sellineName以外的区域，隐藏下拉框
        this.$refs.selectList.style.display = 'none'
        // }
      }
    }
  }
}
</script>

<style lang="scss" scoped>
@import '@/styles/variables.scss';
.search {
  .van-search {
    width: 100%;
  }
}

.start-warehousing {
  padding-top: 20px;
  position: relative;
  height: calc(100vh - 100px);

  .search-reset {
    padding-bottom: 25px;
  }

  .position-date {
    position: absolute;
    top: 5px;
    left: 25px;
  }

  .top {
    padding: 0 0 15px;

    .van-radio-group--horizontal {
      justify-content: space-between;
    }

    .van-radio--horizontal {
      margin-right: 0;
    }
  }

  .dorpdown {
    position: relative;

    .van-button {
      float: right;
      height: 40px;
      background: rgba($color: $white, $alpha: 0.4);
      color: $white;
      border: 0;
      padding: 0 26px;
    }

    .position-drop {
      position: absolute;
      left: 0;
      bottom: -20px;
    }
  }

  .error-message {
    padding: 0;
  }

  // .activered {
  //   span {
  //     color: $red !important;
  //   }
  // }
}

.list-card {
  margin-top: 20px;
  margin-bottom: 80px;

  ul {
    padding: 10px 20px;
    overflow: hidden;
    background-color: rgba($color: $white, $alpha: 0.2);
    // background: linear-gradient(to right, $titleBgColorLinear1, $titleBgColorLinear2 100%);
    border-radius: 5px;
    margin-bottom: 15px;
    font-size: $smallSize;
  }

  li {
    width: 50%;
    float: left;
    line-height: 35px;

    input {
      height: 24px;
      width: 48px;
      background-color: transparent;
      border: 1px solid rgba($color: $white, $alpha: 0.4);
      padding: 0 10px;
    }

    label {
      margin-right: 20px;
      color: rgba($color: $white, $alpha: 0.6);
    }
  }

  li:last-child {
    color: $priceColor;
  }
}

.table-wrap {
  width: 100%;
  overflow: auto;
  width: 100%;
  padding-top: 30px;
  padding-bottom: 20px;
}

.tabStyle {
  white-space: nowrap;
  tr {
    font-size: 10px;
    th {
      height: 40px;
      text-align: center;
      padding: 0 10px;
      opacity: 0.6;
    }
    td {
      height: 40px;
      text-align: center;
      padding: 0 10px;
    }

    td:last-child {
      color: $priceColor;
    }
  }
  .secTable {
    th {
      padding: 0 10px;
    }
    td {
      padding: 0 10px;
    }
  }
  .lastTable {
    th {
      width: 130px;
    }
    td {
      width: 130px;
    }
  }

  input {
    height: 24px;
    width: 48px;
    background-color: transparent;
    border: 1px solid rgba($color: $white, $alpha: 0.4);
    padding: 0 10px;
  }
}

.order-remarks {
  padding: 15px 40px;

  .top-info {
    margin-bottom: 20px;
    margin-top: 35px;
    padding-left: 20px;

    span {
      margin-right: 20px;
    }
  }

  .input-box {
    margin: 0 auto;

    span {
      display: inline-block;
      line-height: 40px;
      width: 50px;
      margin-right: 24px;
      margin-bottom: 15px;
      text-align: right;
    }

    input {
      width: 70px;
      height: 40px;
      border: 1px solid rgba($color: $white, $alpha: 0.6);
      background-color: transparent;
      padding: 0 5px;
      text-align: right;
      box-sizing: border-box;
    }

    .van-cell {
      border: 1px solid #7d7d7f !important;
    }
  }
}

.btn-linear {
  position: fixed;
  bottom: 20px;
  right: 25px;
  height: 35px;
}
</style>

<style lang="scss">
.dorpdown {
  .van-dropdown-menu__bar {
    height: 40px;
  }

  .van-dropdown-menu__title {
    width: 95%;
  }

  .van-dropdown-menu__title::after {
    right: 10px;
  }

  .van-dropdown-menu {
    position: relative;
  }

  .van-dropdown-item {
    position: absolute !important;
    left: 0;
    right: 0;
    height: 230px;
    width: 100%;
    top: 100% !important;
  }

  .van-overlay {
    background: transparent;
  }

  .van-cell {
    padding: 10px;
  }
}
</style>
