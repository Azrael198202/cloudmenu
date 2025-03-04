<template>
  <div>
    <van-row v-if="message !== null && message !== ''" class="error-message">
      <van-col>{{ message }}</van-col>
    </van-row>
    <van-sticky :offset-top="77">
      <van-row class="txt-desc">
        お客様の情報を入力してください
      </van-row>
    </van-sticky>

    <van-row class="package">
      <van-form ref="form" validate-trigger="onSubmit">
        <van-row>
          <van-col span="7"><label for="">お名前</label></van-col>
          <van-col span="17" class="package-input">
            <van-field v-model="name" :rules="rules.name" />
          </van-col>
        </van-row>
        <van-row>
          <van-col span="7"><label for="">電話番号</label></van-col>
          <van-col span="17" class="package-input">
            <van-field v-model="phoneNummber" :rules="rules.PhoneNumber" />
          </van-col>
        </van-row>
        <van-row>
          <van-col span="7"><label for="">受取予定日時</label></van-col>
          <van-col span="10" class="package-input">
            <van-field
              v-model="selectDate"
              type="text"
              :rules="rules.ExpectedDatEAndTime"
              readonly
              @click="isShowDate = true"
            />
            <i class="fa fa-calendar" aria-hidden="true" @click="isShowDate = true" />
          </van-col>
          <van-col span="7" class="package-input">
            <van-field
              v-model="selectTime"
              type="text"
              :rules="rules.ExpectedDatEAndTime"
              readonly
              @click="isShowTime = true"
            />
            <i class="fa fa-clock-o" aria-hidden="true" @click="isShowTime = true" />
          </van-col>
        </van-row>

        <button class="btn btn-linear" @click="next">次へ</button>
      </van-form>
    </van-row>

    <van-popup v-model="isShowDate" position="bottom">
      <van-datetime-picker
        :min-date="minDate"
        type="date"
        confirm-button-text="確認"
        cancel-button-text="キャンセル"
        @confirm="sureDate"
        @cancel="isShowDate = false"
      />
      <!-- :formatter="formatter" -->
    </van-popup>
    <van-popup v-model="isShowTime" position="bottom">
      <van-datetime-picker
        type="time"
        confirm-button-text="確認"
        cancel-button-text="キャンセル"
        @confirm="suretime"
        @cancel="isShowTime = false"
      />
    </van-popup>
  </div>
</template>

<script>
import { insertReceptionTakeoutData } from '@/api/cma/cma082'
import { clearShoppingCart } from '@/utils/auth'
import index from '@/layout/index.vue'

import { formatter } from '@/utils/formatter.js'

export default {
  name: 'Cma082',
  components: { index },
  data() {
    // const formatter = (type, value) => {
    //   if (type === 'year') {
    //     return value
    //   } else if (type === 'month') {
    //     return value
    //   } else if (type === 'day') {
    //     return value
    //   } else if (type === 'hour') {
    //     return value
    //   } else if (type === 'minute') {
    //     return value
    //   }
    //   return value
    // }
    return {
      showError: false,
      message: '',
      minDate: new Date(),
      name: '',
      phoneNummber: '',
      // formatter,
      isShowDate: false,
      isShowTime: false,
      selectDate: '',
      selectTime: '',
      queryParam: {
        // 选择中座位集合
        selectedSeatList: [],
        // 选择中有无座位
        selectedSeatOnoff: '0',
        // 坐席选择mode
        seatSelectMode: '0',
        // 商品menu分类表示顺序
        menuLineNumber: '',
        // 商品menu注文番号
        menuOrderNumber: '',
        // 受付区分
        receptionKbn: '',
        // 坐席集合
        seatList: [
          // 选择坐席Flag
          // selectedSeatFlg:'0',
          // 组设置模式
          // groupMode
        ]
      },
      rules: {
        name: [{ required: true, message: this.$msgUtil.get('E_00020', '氏名') }],
        PhoneNumber: [{ required: true, message: this.$msgUtil.get('E_00020', '電話番号') }],
        ExpectedDatEAndTime: [{ required: true, message: this.$msgUtil.get('E_00020', '受取予定日時') }]
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
  },
  methods: {
    // ----日期选择-------------------------------------------------
    next() {
      this.$refs['form'].validate().then(() => {
        // 校验成功时
        this.queryParam.takeoutName = this.name
        this.queryParam.takeoutTel = this.phoneNummber
        this.queryParam.takeoutReceiptTime = this.selectDate + ' ' + this.selectTime
        insertReceptionTakeoutData(this.queryParam).then(response => {
          if (response.status === 200) {
            // 清空购物车
            clearShoppingCart()
            this.saveSelectToStore(response)
            this.$router.push({
              path: '/Employee/CommoditySearchMethod'
            })
          } else if (response.status === 601) {
            this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
          } else if (response.status === 602) {
            this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
          }
        })
      })
    },
    // session保存
    saveSelectToStore(response) {
      sessionStorage.setItem(
        'seatInfo',
        JSON.stringify({
          receptionKbn: '002',
          mainSeat: response.receptionList[0]
        })
      )
    },
    // 确定日期
    sureDate(val) {
      this.selectDate = formatter.formatDateSelf(val, 'yyyy/MM/dd')
      this.isShowDate = false
    },
    suretime(val) {
      this.selectTime = val
      this.isShowTime = false
    },

    // 取消
    cancelDate() {
      this.isShowDate = false
    }
  }
}
</script>

<style lang="scss" scoped>
@import '@/styles/variables.scss';

.package {
  padding: 0 25px;
  height: 32px;
  line-height: 32px;
  font-size: $smallSize;
  height: calc(100vh - 130px);

  .van-col {
    position: relative;
  }

  .package-input {
    .van-cell {
      padding: 0 5px;
      border: 0;
    }
  }

  i {
    font-size: 18px;
    position: absolute;
    right: 10px;
    top: 15px;
    transform: translateY(-50%);
    opacity: 0.4;
  }

  .btn {
    position: fixed;
    bottom: 20px;
    right: 25px;
    width: 80px;
    height: 35px;
    border: 0;
    border-radius: 5px;
  }
}
</style>

<style lang="scss">
@import '@/styles/variables.scss';

.package-input {
  .van-field__control {
    color: $white;
    height: 30px;
    background-color: transparent;
    border: 1px solid rgba($color: $white, $alpha: 0.4);
    padding-left: 10px;
    width: 100%;
    box-sizing: border-box;
  }

  .van-cell__value--alone {
    height: 55px;
  }
}

.package {
  .van-form > .van-row:nth-child(3) {
    .package-input .van-cell__value--alone {
      height: 77px;
    }
  }
}
</style>
