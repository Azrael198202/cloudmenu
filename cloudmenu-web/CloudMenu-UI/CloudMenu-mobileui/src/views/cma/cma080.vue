<template>
  <div ref="sellineName" @click="closeSel">
    <van-row v-if="message != ''" class="error-message">
      <van-col>{{ message }}</van-col>
    </van-row>
    <van-sticky :offset-top="77">
      <van-row class="txt-desc">
        座席選択して下さい
      </van-row>
    </van-sticky>

    <div class="seatList">
      <van-checkbox-group v-model="seatChkList">
        <van-cell-group>
          <van-cell
            v-for="(item, index) in seatList"
            :key="index"
            clickable
            :title="item.seatName"
            @click.stop="seatChoose(index, item, $event)"
          >
            <template #icon>
              <div :class="item.seatStatusKbn == '001' ? 'green' : 'blue'" />
            </template>
            <template #right-icon>
              <van-checkbox ref="checkboxes" v-model="item.selectedSeatFlg" :name="item" shape="square" />
            </template>
          </van-cell>
        </van-cell-group>
      </van-checkbox-group>
    </div>
    <van-cell :disabled="seatChkList.length > 0" class="seatCell" title="持ち帰り" @click.stop="pack" />
    <van-row class="select-seat">
      <van-col span="5" class="icon">
        <i />
        <span>人数</span>
      </van-col>

      <van-col span="8" class="input-info">
        <div v-if="showPeopleFlg">
          <van-row class="total-people">
            <span>人数:</span>
            <span>{{ peopleSize }}名</span>
          </van-row>
          <van-row class="people-detail">
            <span>男 {{ seatPeopleMan }} 名</span>
            <span>女 {{ seatPeopleWoman }} 名</span>
            <span>子供 {{ seatPeopleChildren }} 名</span>
          </van-row>
        </div>
      </van-col>

      <van-col span="11">
        <van-button :disabled="seatChkList.length == 0" class="btn cancel fl" @click.stop="reception()">
          受付
        </van-button>
        <van-button
          type="primary"
          :disabled="seatChkList.length == 0"
          class="btn fr btn-linear"
          @click.stop="nextStep()"
        >
          注文
        </van-button>
      </van-col>
    </van-row>

    <van-popup v-model="showDialog" class="order-remarks">
      <van-form ref="form">
        <van-row class="title">
          人数
        </van-row>
        <van-row class="input-box">
          <van-col span="8">
            <span>男：</span>
          </van-col>
          <van-col span="8">
            <van-field v-model="seatChkItem.seatPeopleMan" type="digit" :maxlength="2" />
          </van-col>
          <van-col span="8">
            <span style="text-align:left;margin-left:15%">名</span>
          </van-col>
        </van-row>
        <van-row class="input-box">
          <van-col span="8">
            <span>女：</span>
          </van-col>
          <van-col span="8">
            <van-field v-model="seatChkItem.seatPeopleWoman" type="digit" :maxlength="2" />
          </van-col>
          <van-col span="8">
            <span style="text-align:left;margin-left:15%">名</span>
          </van-col>
        </van-row>
        <van-row class="input-box">
          <van-col span="8">
            <span>子供：</span>
          </van-col>
          <van-col span="8">
            <van-field v-model="seatChkItem.seatPeopleChildren" type="digit" :maxlength="2" />
          </van-col>
          <van-col span="8">
            <span style="text-align:left;margin-left:15%">名</span>
          </van-col>
        </van-row>
        <van-row class="input-box">
          <van-col span="8">
            <span>人数：</span>
          </van-col>
          <van-col span="8">
            <span style="text-align:left">{{ sumPeopleNum() }}名</span>
          </van-col>
        </van-row>

        <van-row class="select-down">
          <van-row class="select-input">
            <input v-model="seatChkItem.receptionRemarks" type="text" @click.stop="searchInfo" />
            <i class="fa fa-caret-down" aria-hidden="true" />
          </van-row>

          <ul ref="selectList">
            <!-- 注意！注意！注意！这里循环遍历的是items，不再是data里的list数组 -->
            <li v-for="(item, index) in items" :key="index" @click.stop="selectCurrent(item.value, $event)">
              <span>{{ item.text }}</span>
            </li>
          </ul>
        </van-row>

        <van-row class="row-btn">
          <van-button type="default" class="cancel fl" @click.stop="saveOK()">OK</van-button>
        </van-row>
      </van-form>
    </van-popup>
  </div>
</template>

<script>
import { getList, insertReceptionInsideData } from '@/api/cma/cma080'
import { selectOrderData } from '@/api/cma/cma120'
import { clearShoppingCart } from '@/utils/auth'
import { searchKuBun } from '@/api/common'
export default {
  name: 'SeatSelection',
  data() {
    return {
      message: null,
      receptionKbn: '001', // 外卖或者店内吃 001：店内吃
      // 坐席集合
      seatList: [],
      seatChkList: [],
      // 点击显示的下方人数数据源
      seatChkHas: '0', // 是否选择座位 0：没有  1：有
      seatChkKbn: '0', // 座席選択モード 0：（選択なし）を設定；1：（新規）を設定；2：（追加）を設定
      showPeopleFlg: false,
      peopleSize: null,
      seatPeopleMan: null,
      seatPeopleWoman: null,
      seatPeopleChildren: null,
      receptionRemarks: null,
      // 控制POP弹窗下拉菜单显示
      showDialog: false,
      // 弹出窗口数据源
      seatChkItem: {
        seatPeopleMan: null,
        seatPeopleWoman: null,
        seatPeopleChildren: null,
        peopleSize: null,
        receptionRemarks: null
      },
      remakeOption: [],
      show: false
    }
  },
  computed: {
    // 计算差异值=输入库存数-理论库存数
    sumPeopleNum() {
      return function() {
        this.seatChkItem.peopleSize =
          parseInt(this.seatChkItem.seatPeopleMan ? this.seatChkItem.seatPeopleMan : 0) +
          parseInt(this.seatChkItem.seatPeopleWoman ? this.seatChkItem.seatPeopleWoman : 0) +
          parseInt(this.seatChkItem.seatPeopleChildren ? this.seatChkItem.seatPeopleChildren : 0)
        return this.seatChkItem.peopleSize
      }
    },
    // 过滤方法
    items: function() {
      var _search = this.seatChkItem.receptionRemarks
      if (_search) {
        // 不区分大小写处理
        var reg = new RegExp(_search, 'ig')
        // es6 filter过滤匹配，有则返回当前，无则返回所有
        return this.remakeOption.filter(function(e) {
          // 匹配所有字段
          return Object.keys(e).some(function(key) {
            return e[key].match(reg)
          })
        })
      }
      return this.remakeOption
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
    this.init()
    this.searchKuBun()
  },
  methods: {
    // 初始化事件
    init() {
      this.seatChkList = []
      getList().then(response => {
        if (response.status === 200) {
          if (response.seatList.length === 0) {
            this.$msgUtil.messageNew('E_00010', '座席データ全件', this)
            return
          }
          for (let i = 0; i < response.seatList.length; i++) {
            if (response.seatList[i].receptionBranchNumber === '001') {
              response.seatList[i].groupMode = '1' // （G親）
            } else if (!response.seatList[i].receptionBranchNumber) {
              response.seatList[i].groupMode = '0' // （无设定）
            } else {
              response.seatList[i].groupMode = '2' // （G子）
            }
            // 座位单选框
            response.seatList[i].selectedSeatFlg = false
          }
          this.seatList = response.seatList
        } else if (response.status === 601) {
          this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
        } else if (response.status === 602) {
          this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
        }
      })
    },
    // 座位行点击事件
    seatChoose(index, item) {
      this.message = null // 清空画面上部错误消息
      // 隐藏并清空下方数据
      this.showPeopleFlg = false
      this.peopleSize = null
      this.seatPeopleMan = null
      this.seatPeopleWoman = null
      this.seatPeopleChildren = null
      this.receptionRemarks = null
      this.$refs.checkboxes[index].toggle()
      item.selectedSeatFlg = !this.$refs.checkboxes[index].checked
      this.seatChkHas = '0' // 是否选择座位 0：没有  1：有
      this.seatChkKbn = '0' // 座席選択モード 0：（選択なし）を設定；1：（新規）を設定；2：（追加）を設定
      if (item.selectedSeatFlg || (!item.selectedSeatFlg && this.seatChkList.length > 1)) {
        this.seatChkHas = '1'
        this.seatChkKbn = '1'
      }
      // 判断座席選択モード
      if (this.seatChkHas === '1') {
        if (item.selectedSeatFlg && item.seatStatusKbn !== '001') {
          this.seatChkKbn = '2'
        } else {
          for (let i = 0; i < this.seatChkList.length; i++) {
            const el = this.seatChkList[i]
            if (!item.selectedSeatFlg && el.seatNumber === item.seatNumber) {
              continue
            }
            // 存在一个不是空闲，座席選択モード就是2
            if (el.seatStatusKbn !== '001') {
              this.seatChkKbn = '2'
              break
            }
          }
        }
      }
      if (this.seatChkKbn === '2') {
        // 显示主桌的人数和详细
        if (item.selectedSeatFlg && item.groupMode === '1') {
          this.showPeopleFlg = true
          this.peopleSize = item.seatPeopleMan + item.seatPeopleWoman + item.seatPeopleChildren
          this.seatPeopleMan = item.seatPeopleMan
          this.seatPeopleWoman = item.seatPeopleWoman
          this.seatPeopleChildren = item.seatPeopleChildren
          this.receptionRemarks = item.receptionRemarks
        } else {
          for (let i = 0; i < this.seatChkList.length; i++) {
            const el = this.seatChkList[i]
            if (!item.selectedSeatFlg && el.seatNumber === item.seatNumber) {
              continue
            }
            if (el.groupMode === '1') {
              this.showPeopleFlg = true
              this.peopleSize = el.seatPeopleMan + el.seatPeopleWoman + el.seatPeopleChildren
              this.seatPeopleMan = el.seatPeopleMan
              this.seatPeopleWoman = el.seatPeopleWoman
              this.seatPeopleChildren = el.seatPeopleChildren
              this.receptionRemarks = el.receptionRemarks
              break
            }
          }
        }
      }
    },
    // 受付按钮点击
    async reception() {
      this.message = null
      this.seatChkItem.seatPeopleMan = null
      this.seatChkItem.seatPeopleWoman = null
      this.seatChkItem.seatPeopleChildren = null
      this.seatChkItem.peopleSize = null
      this.seatChkItem.receptionRemarks = null
      // 调用后台接口判断
      for (let i = 0; i < this.seatChkList.length; i++) {
        const el = this.seatChkList[i]
        if (el.seatStatusKbn === '002') {
          const data = { receptionNumber: el.receptionNumber, receptionBranchNumber: el.receptionBranchNumber }
          await selectOrderData(data).then(response => {
            if (response.status === 200) {
              if (response.dataCount !== 0) {
                this.$msgUtil.messageNew('E_00250', '', this)
                return
              }
            } else if (response.status === 601) {
              this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
            } else if (response.status === 602) {
              this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
            }
          })
        }
      }
      this.seatChkItem.seatPeopleMan = this.seatPeopleMan
      this.seatChkItem.seatPeopleWoman = this.seatPeopleWoman
      this.seatChkItem.seatPeopleChildren = this.seatPeopleChildren
      this.seatChkItem.peopleSize = this.peopleSize
      this.seatChkItem.receptionRemarks = this.receptionRemarks
      this.showDialog = true
      this.closeSel()
    },
    // 弹出框OK按钮
    saveOK() {
      this.message = null
      if (
        (this.seatChkItem.seatPeopleMan === '' ||
          this.seatChkItem.seatPeopleMan === '0' ||
          this.seatChkItem.seatPeopleMan === 0) &&
        (this.seatChkItem.seatPeopleWoman === '' ||
          this.seatChkItem.seatPeopleWoman === '0' ||
          this.seatChkItem.seatPeopleWoman === 0) &&
        (this.seatChkItem.seatPeopleChildren === '' ||
          this.seatChkItem.seatPeopleChildren === '0' ||
          this.seatChkItem.seatPeopleChildren === 0)
      ) {
        this.$msgUtil.messageNew('W_00070', null, this).then(() => {
          this.saveOKNext()
        })
        return
      } else {
        this.saveOKNext()
      }
    },
    saveOKNext() {
      this.$refs['form'].validate().then(() => {
        var minNumber = this.mathMin(this.seatList)
        // 赋值给当前选中的最小桌 seatList
        this.seatList[minNumber].seatPeopleMan = this.seatChkItem.seatPeopleMan
        this.seatList[minNumber].seatPeopleWoman = this.seatChkItem.seatPeopleWoman
        this.seatList[minNumber].seatPeopleChildren = this.seatChkItem.seatPeopleChildren
        this.seatList[minNumber].receptionRemarks = this.seatChkItem.receptionRemarks
        this.showDialog = false
        // 設計書通り、渡すパラメータを構築
        const seatChkListSub = []
        let args = ''
        for (let index = 0; index < this.seatChkList.length; index++) {
          if (index === this.seatChkList.length - 1) {
            args += this.seatChkList[index].seatName
          } else {
            args += this.seatChkList[index].seatName + ';'
          }
          const el = {}
          el.selectedSeatNumber = this.seatChkList[index].seatNumber
          seatChkListSub.push(el)
        }
        var submitData = {
          selectedSeatList: seatChkListSub,
          selectedSeatOnoff: this.seatChkHas,
          seatSelectMode: this.seatChkKbn,
          menuLineNumber: '',
          menuOrderNumber: '',
          receptionKbn: this.receptionKbn,
          seatList: this.seatList
        }
        insertReceptionInsideData(submitData).then(response => {
          if (response.status === 200) {
            this.init() // 初始化
            this.$msgUtil.messageNew('I_00010', args, this) // 消息
            // 调用后台接口
            this.peopleSize = this.seatChkItem.peopleSize
            this.seatPeopleMan = this.seatChkItem.seatPeopleMan === '' ? 0 : this.seatChkItem.seatPeopleMan
            this.seatPeopleWoman = this.seatChkItem.seatPeopleWoman === '' ? 0 : this.seatChkItem.seatPeopleWoman
            this.seatPeopleChildren =
              this.seatChkItem.seatPeopleChildren === '' ? 0 : this.seatChkItem.seatPeopleChildren
            this.showPeopleFlg = true
          } else if (response.status === 601) {
            this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
          } else if (response.status === 602) {
            this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
          }
        })
      })
    },
    // 注文按钮点击
    nextStep() {
      if (this.seatChkList.length !== 1) {
        this.$msgUtil.messageNew('E_00230', '', this)
        return
      }
      if (this.seatChkList[0].seatStatusKbn === '001') {
        this.$msgUtil.messageNew('E_00240', '', this)
        return
      }
      this.receptionKbn = '001'
      // 放入缓存
      this.saveSelectToStore(0)
      clearShoppingCart()
      this.$router.push({
        path: '/Employee/CommoditySearchMethod'
      })
    },
    // 放入缓存
    saveSelectToStore(minIndex) {
      sessionStorage.setItem(
        'seatInfo',
        JSON.stringify({
          receptionKbn: this.receptionKbn,
          mainSeat: this.seatChkList.length === 0 ? null : this.seatChkList[minIndex]
        })
      )
    },
    pack() {
      // 活性时的处理
      this.receptionKbn = '002'

      this.$router.push({
        path: '/Employee/packageAcceptance'
      })
    },
    // 查看最小的桌号
    mathMin(arrs) {
      if (arrs.length === 0) {
        return null
      }
      var minNumberSeatIndex = null
      for (var i = 0; i < arrs.length; i++) {
        if (arrs[i].selectedSeatFlg) {
          if (minNumberSeatIndex === null) {
            minNumberSeatIndex = i
          }
          if (arrs[i].seatNumber < arrs[minNumberSeatIndex].seatNumber) {
            minNumberSeatIndex = i
          }
        }
      }
      return minNumberSeatIndex
    },
    searchKuBun() {
      this.remakeOption = []
      searchKuBun({ categoryClassNumber: '070' }).then(response => {
        for (let i = 0; i < response.categoryList.length; i++) {
          const item = {}
          item.value = response.categoryList[i].categoryKbn
          item.text = response.categoryList[i].categoryKbnName
          this.remakeOption.push(item)
        }
      })
    },

    // 选中当前检索项
    selectCurrent(value, event) {
      if (this.showDialog) {
        var menuText = event.currentTarget.innerText
        this.seatChkItem.receptionRemarks = menuText
        this.$refs.selectList.style.display = 'none'
      }
    },

    // 输入框输入事件
    searchInfo() {
      if (this.showDialog) {
        this.$refs.selectList.style.display = 'block'
      }
    },

    closeSel(event) {
      const currentCli = this.$refs.sellineName
      if (currentCli && this.showDialog) {
        // 点击到了id为sellineName以外的区域，隐藏下拉框
        if (this.$refs.selectList) {
          this.$refs.selectList.style.display = 'none'
        }
      }
    }
  }
}
</script>

<style lang="scss" scoped>
@import '@/styles/variables.scss';

.txt-desc {
  padding-left: 40px;
}

.list-info {
  padding-bottom: 150px;
}

// 人数
.select-seat-wrap {
  height: 100px;
}

.select-seat {
  width: 100%;
  height: 100px;
  background-color: $mainColor;
  position: fixed;
  margin-top: 100px;
  bottom: 0;
  left: 0;
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0 25px 0 40px;
  box-sizing: border-box;

  .icon {
    margin-right: 8px;
    border: 1px solid $inputBorder;
    padding: 10px 0;
    border-radius: 3px;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;

    i {
      display: block;
      width: 35px;
      height: 23px;
      background: url('../../assets/images/group.png') center center no-repeat;
      background-size: 100% 100%;
      margin: 0 auto 8px;
    }
  }

  .input-info {
    display: flex;
    align-items: flex-start;
    flex-direction: column;
    vertical-align: top;

    .total-people {
      font-size: $titleSize;
      margin-bottom: 10px;

      span:last-child {
        margin-left: 10px;
      }
    }

    .people-detail {
      font-size: $moreSmallSize;

      span {
        display: inline-block;
        width: 50%;
        line-height: 20px;
        color: rgba($color: $white, $alpha: 0.6);
      }
    }
  }
}

.btn {
  border-radius: 10px;
  border: 0 !important;
  width: 60px;
  height: 40px;
  border-radius: 5px;
}

.van-button--default {
  background: $inputBorder;
}

.blue {
  box-shadow: 1px 1px 3px 1px rgb(255 255 255 / 30%);
  width: 10px;
  height: 10px;
  border-radius: 50%;
  background-color: blue;
  margin-top: 6px;
  margin-right: 10px;
}

.green {
  box-shadow: 1px 1px 3px 1px rgb(255 255 255 / 30%);
  width: 10px;
  height: 10px;
  border-radius: 50%;
  background-color: #888888;
  margin-top: 6px;
  margin-right: 10px;
}

.seatCell {
  position: fixed;
  bottom: 100px;
  left: 0;
  background: $mainColor;
}

.order-remarks {
  padding: 20px 36px;
  font-size: 14px;
  max-height: 70%;

  .input-box {
    margin: 0 auto;

    span {
      display: inline-block;
      line-height: 38px;
      width: 50px;
      margin-bottom: 15px;
      text-align: right;
    }

    input {
      width: 65px;
      height: 40px;
      border: 1px solid rgba($color: $white, $alpha: 0.6);
      margin-right: 10px;
      background-color: transparent;
      padding: 0 5px;
      box-sizing: border-box;
    }

    .van-cell {
      padding: 6px 10px;
      border: 1px solid #7d7d7f !important;
    }
  }

  .row-btn {
    display: flex;
    justify-content: center;
  }

  .van-overlay {
    height: auto;
  }

  button {
    margin-bottom: 0;
    margin-top: 60px;
  }
}
.seatList {
  margin-bottom: 150px;

  .van-cell {
    font-size: 14px;
  }
}

.datelistStyle {
  position: relative;
  .van-icon {
    transform: rotate(90deg);
    position: absolute;
    right: 10px;
    top: 15px;
  }

  input {
    width: 100%;
    height: 40px;
    border-radius: 5px;
    background-color: #1c1a21;
    border: 1px solid #ced4da;
    padding-left: 5px;
  }
}

input::-webkit-calendar-picker-indicator {
  background-color: inherit;
}
</style>

<style lang="scss">
@import '@/styles/variables.scss';
.order-remarks {
  .van-field__control {
    color: $white !important;
  }

  .van-dropdown-item__content {
    height: 95px;
  }

  .input-box {
    .van-field__control {
      text-align: right;
    }
  }
}
</style>
