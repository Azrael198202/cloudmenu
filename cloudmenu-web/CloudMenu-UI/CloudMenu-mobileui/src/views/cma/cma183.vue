<template>
  <div>
    <van-row v-if="message !== null && message !== ''" class="error-message">
      <van-col>{{ message }}</van-col>
    </van-row>
    <van-row class="storage-qua">
      <van-row class="title">
        {{ query.categoryKbnNameSon }}
        <i>{{ query.categoryKbnName }}</i>
      </van-row>
      <van-form :show-error-message="true" @submit="onSubmit">
        <van-field
          v-model="storingQuantity"
          class="storing-input"
          placeholder="入庫数"
          type="number"
          maxlength="6"
          input-align="right"
          style="margin-right:0px;"
          :rules="rules.storingQuantity"
        />

        <van-field
          v-model="remarks"
          rows="2"
          autosizetype="textarea"
          maxlength="200"
          placeholder="備考"
          :rules="rules.remarks"
          style="margin-right:0px;"
          show-word-limit
        />

        <van-row class="btn">
          <van-button type="primary" class="btn-linear" native-type="submit">OK</van-button>
        </van-row>
      </van-form>
    </van-row>
  </div>
</template>
<script>
import { addNyukoProduct, addNyukoMaterial, addNyukoEquipment } from '@/api/cma/cma183'
export default {
  name: 'EnterTheStockInQuantity',
  data() {
    return {
      message: null,
      query: {
        itemKbn: '',
        itemKbnName: '',
        storingDate: '',
        categoryKbn: '',
        categoryKbnName: '',
        categoryKbnSon: '',
        categoryKbnNameSon: ''
      },
      storingQuantity: null,
      remarks: '',
      rules: {
        storingQuantity: [{ required: true, message: this.$msgUtil.get('E_00020', '入庫数') }],
        remarks: [{ required: true, message: this.$msgUtil.get('E_00020', '備考') }]
      }
    }
  },
  created() {
    if (
      sessionStorage.getItem('180Query') &&
      sessionStorage.getItem('180Query') !== null &&
      sessionStorage.getItem('180Query') !== undefined
    ) {
      this.query = JSON.parse(sessionStorage.getItem('180Query'))
    }
  },
  methods: {
    onSubmit() {
      this.message = null
      const data = {}
      data.storingDate = this.query.storingDate.replaceAll('/', '')
      data.itemKbn = this.query.categoryKbn
      data.productNumber = this.query.categoryKbnSon
      data.storingQuantity = this.storingQuantity
      data.remarks = this.remarks
      if (this.query.itemKbn === '031') {
        addNyukoProduct(data).then(response => {
          if (response.status === 200) {
            // "入庫の追加"
            this.$msgUtil.messageNew('I_00100', '入庫の追加', this)
            // 跳转画面
            this.$router.push({ path: '/Employee/SelectionOfIncomingoods' })
          } else if (response.status === 601) {
            this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
          } else if (response.status === 602) {
            this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
          }
        })
      } else if (this.query.itemKbn === '032') {
        addNyukoMaterial(data).then(response => {
          if (response.status === 200) {
            // "入庫の追加"
            this.$msgUtil.messageNew('I_00100', '入庫の追加', this)
            // 跳转画面
            this.$router.push({ path: '/Employee/SelectionOfIncomingoods' })
          } else if (response.status === 601) {
            this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
          } else if (response.status === 602) {
            this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
          }
        })
      } else if (this.query.itemKbn === '033') {
        addNyukoEquipment(data).then(response => {
          if (response.status === 200) {
            // "入庫の追加"
            this.$msgUtil.messageNew('I_00100', '入庫の追加', this)
            // 跳转画面
            this.$router.push({ path: '/Employee/SelectionOfIncomingoods' })
          } else if (response.status === 601) {
            this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
          } else if (response.status === 602) {
            this.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
          }
        })
      }
    }
  }
}
</script>

<style lang="scss" scoped>
@import '@/styles/variables.scss';

.storage-qua {
  padding: 0 25px;

  .title {
    line-height: 46px;
    font-size: $titleSize;

    i {
      font-size: $smallSize;
      font-style: normal;
      color: rgba($color: $white, $alpha: 0.5);
      margin-left: 25px;
    }
  }

  .van-cell {
    background-color: $white;
    border-radius: 3px;
    margin-bottom: 15px;
    padding: 5px 15px;
  }

  .btn {
    position: fixed;
    bottom: 20px;
    right: 20px;
  }

  .btn-linear {
    height: 35px;
    width: 80px;
  }

  .van-cell {
    background: transparent;
    padding: 0;
    border: 0;
  }
}
</style>

<style lang="scss">
@import '@/styles/variables.scss';

.storage-qua {
  input {
    background-color: $mainDialogColor;
    height: 36px;
    padding: 5px 15px;
  }
  input::-webkit-input-placeholder {
    color: $white;
  }
  input::-moz-input-placeholder {
    color: $white;
  }
  input::-ms-input-placeholder {
    color: $white;
  }

  textarea {
    background: $white;
    height: 58px;
    padding: 5px 15px;
  }

  .van-field--error .van-field__control,
  .van-field--error .van-field__control::placeholder {
    color: $searchColor;
  }
}
</style>
