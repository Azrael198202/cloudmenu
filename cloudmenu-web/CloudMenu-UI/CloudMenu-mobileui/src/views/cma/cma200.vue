<template>
  <div>
    <van-row v-if="message !== null && message !== ''" class="error-message">
      <van-col>{{ message }}</van-col>
    </van-row>
    <van-sticky :offset-top="77">
      <van-row class="txt-desc">
        {{ name }}
        <span>注文可能数</span>
      </van-row>
    </van-sticky>
    <van-row class="change-quantity">
      <div v-for="item in dataList" :key="item.productNumber + item.productSetNumber">
        <van-cell>
          <van-row>
            <van-col span="8">
              <!-- 显示一人前 -->
              <span v-if="item.setExistenceFlg === '1' && item.productSetName !== null && item.productSetName !== ''">
                {{ item.productNameCh }}
                <b class="meal">{{ item.productSetName }}</b>
              </span>
              <span v-else>{{ item.productNameCh }}</span>
            </van-col>
            <van-col span="8">
              <van-dropdown-menu>
                <van-dropdown-item v-model="item.productLimitedKbnSw" :options="kbnList" />
              </van-dropdown-menu>
            </van-col>
            <van-col span="8">
              <div :class="item.productLimitedQuantity == 0 ? 'nonum' : ''" class="fr">
                <van-stepper
                  v-model="item.productLimitedQuantity"
                  class="number-stepper"
                  integer
                  min="0"
                  :disabled="item.productLimitedKbnSw === '900'"
                />
              </div>
            </van-col>
          </van-row>
        </van-cell>
      </div>
    </van-row>

    <van-row class="btn-login">
      <button type="primary" class="btn-linear" @click="submit">登録</button>
    </van-row>
  </div>
</template>

<script>
import { selectShohinTypeKbnData, updateLimitedQuantity } from '@/api/cma/cma200'
import { searchKuBun } from '@/api/common'
export default {
  name: 'Cma200',
  data() {
    return {
      code: '',
      name: '',
      dataList: [],
      kbnList: [],
      message: null,
      show: false
    }
  },
  created() {
    this.code = this.$route.query.code
    this.name = this.$route.query.name
    this.init(this.code)
    this.searchKuBun()
  },
  methods: {
    // 检索
    init(code) {
      const thisObj = this
      thisObj.dataList = []
      const query = { productTypeKbn: code }
      selectShohinTypeKbnData(query).then(response => {
        if (response.status === 200) {
          const list = response.productTypeKbnList
          // 数量限定区分スイッチ
          for (let index = 0; index < list.length; index++) {
            if (list[index].limitedKbn === '900') {
              list[index].productLimitedKbnSw = '0'
              list[index].productLimitedQuantity = 0
            } else {
              list[index].productLimitedKbnSw = '1'
            }
            thisObj.dataList.push(list[index])
          }
        } else if (response.status === 601) {
          thisObj.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
        } else if (response.status === 602) {
          thisObj.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
        }
      })
    },
    submit() {
      // 构建接口参数
      const data = {}
      data.selectedTypeKbnList = []
      const dataSon = {}
      dataSon.productTypeKbn = this.code
      dataSon.productTypeKbnName = this.name
      dataSon.productList = this.dataList
      data.selectedTypeKbnList.push(dataSon)
      // 调用注文可能数変更一時領域接口
      updateLimitedQuantity(data).then(response => {
        if (response.status === 200) {
          this.$msgUtil
            .messageBox('I_00110', '', this)
            .then(() => {
              this.init(this.code)
            })
            .catch(() => {
              return
            })
        } else if (response.status === 601) {
          this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
        } else if (response.status === 602) {
          this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
        }
      })
    },
    // 开关切换
    switchChange(item) {
      if (item.productLimitedKbnSw === '900') {
        item.productLimitedQuantity = 0
      }
    },
    searchKuBun() {
      searchKuBun({ categoryClassNumber: '080' }).then(response => {
        for (let i = 0; i < response.categoryList.length; i++) {
          const item = {}
          item.value = response.categoryList[i].categoryKbn
          item.text = response.categoryList[i].categoryKbnName
          this.kbnList.push(item)
        }
      })
    }
  }
}
</script>

<style lang="scss" scoped>
@import '@/styles/variables.scss';

.txt-desc {
  span {
    font-size: $moreSmallSize;
    float: right;
  }
}

.change-quantity {
  margin-bottom: 125px;

  .van-cell {
    padding: 0 25px;
    min-height: 50px;
    height: auto;
    justify-content: space-between;
  }

  span:first-child {
    width: 110px;
    display: inline-block;
  }

  .meal {
    font-weight: normal;
    font-size: $moreSmallSize;
    color: rgba($color: $white, $alpha: 0.6);
    display: block;
    line-height: 14px;
  }
}
</style>

<style lang="scss">
@import '@/styles/variables.scss';

.number-stepper {
  .van-stepper__input {
    text-align: right;
    padding-right: 5px;
  }
}
.change-quantity {
  .van-cell__value--alone {
    display: flex;
    align-items: center;
    // flex-direction: column;

    > .van-row {
      width: 100%;
      margin: 10px 0;
      display: flex;
      align-items: center;
    }
  }

  .van-dropdown-menu__title {
    width: 100%;
    height: auto;
    position: relative;
    display: block;
    border-radius: 3px;
  }

  .van-dropdown-menu__title::after {
    right: 15px;
  }

  .van-dropdown-item--down {
    width: 100%;
  }

  .van-dropdown-item__content {
    max-height: 30%;
  }

  //   以下是改变switch开关
  .van-switch {
    width: 38px;
    height: 20px;
    margin-right: 30px;
    background-color: rgba($color: $white, $alpha: 0.6);
  }

  .van-switch__node {
    height: 22px;
    width: 22px;
    top: -1px;
  }

  .van-switch--on .van-switch__node {
    transform: translateX(16px);
    background-color: $titleColor;
  }

  .van-switch--on {
    background-color: $switchColor;
  }

  .van-dropdown-menu__bar {
    background: transparent;
    border: 1px solid $inputBorder;

    .van-dropdown-menu__title {
      color: $white;
    }
  }

  //   以下是调整步进器
  .van-stepper__minus,
  .van-stepper__plus {
    background-color: transparent;
    color: $priceColor;
    border-radius: 0;
    border: 1px solid $priceColor;
  }

  .van-stepper__minus {
    margin-right: 5px !important;
  }

  .van-stepper__plus {
    margin-right: 5px;
  }

  .van-stepper {
    .van-stepper__input {
      float: right;
      width: 40px;
      background: transparent;
      color: $white;
      border: 1px solid rgba($color: $white, $alpha: 0.5);
    }
  }

  .van-stepper__minus--disabled,
  .van-stepper__plus--disabled {
    background-color: transparent;
    border: 1px solid rgba($color: $white, $alpha: 0.5);
    border-radius: 0;
    color: rgba($color: $white, $alpha: 0.5);
  }

  .nonum {
    .van-stepper {
      .van-stepper__plus {
        color: rgba($color: $white, $alpha: 0.5);
        border: 1px solid rgba($color: $white, $alpha: 0.5);
      }
    }
  }
}
</style>
