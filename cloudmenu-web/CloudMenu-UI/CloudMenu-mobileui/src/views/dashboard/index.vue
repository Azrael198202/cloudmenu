<template>
  <div class="goods">
    <van-button v-show="showLogin" type="primary" @click="login">{{ $t('demo.login') }}</van-button>
    <van-button type="primary" @click="agentRegister">经纪人注册</van-button>
    <van-button type="primary">{{ $t('message.test') }}</van-button>

    <div>
      <div>
        演示功能
        <div>
          <span>多语言</span>
        </div>
        <div style="margin-top: 20px">
          <van-radio-group ref="lang" v-model="lang" size="mini" @change="changeLang">
            <van-radio name="zh">中文</van-radio>
            <van-radio name="ja">日本語</van-radio>
          </van-radio-group>
        </div>

        <div>
          <span>Mock数据</span>
        </div>
        <div style="margin-top: 20px">
          <van-button type="success" @click="getListData">获取mock数据</van-button>
          <div v-for="(person, index) in persons" :key="index">{{ person.name }}</div>
        </div>
        <div>
          <span>消息提示</span>
        </div>
        <div style="margin-top: 20px">
          <van-button type="success" @click="getMessageContent">获取消息内容</van-button>
          {{ messageContent }}
        </div>
        <div style="margin-top: 20px">
          <van-button type="success" @click="showMessage">弹出消息提示</van-button>
        </div>
        <div style="margin-top: 20px">
          <van-button type="success" @click="showMessageBox">弹出消息框</van-button>
        </div>

        <div>
          <span>表单验证</span>
        </div>

        <van-form>
          <van-field
            v-model="loginForm.userName"
            :placeholder="$t('demo.userName')"
            :rules="[{ required: true, message: this.$msgUtil.get('E_0001'), trigger: 'onChange' }]"
          />
          <van-field
            v-model="loginForm.mailAddress"
            :placeholder="$t('demo.mailAddress')"
            :rules="[
              { required: true, message: this.$msgUtil.get('E_0004'), trigger: 'onChange' },
              { validator: vantValidEmail, message: this.$msgUtil.get('E_0005'), trigger: 'onChange' }
            ]"
          />
          <van-field
            v-model="loginForm.confirmMailAddress"
            :placeholder="$t('demo.confirmMailAddress')"
            :rules="[
              { required: true, message: this.$msgUtil.get('E_0006'), trigger: 'onChange' },
              { validator: confirmEmail, message: this.$msgUtil.get('E_0007'), trigger: 'onChange' }
            ]"
          />
          <van-field v-model="loginForm.password" :placeholder="$t('demo.password')" :rules="rules.password" />
          <van-field
            v-model="loginForm.confirmPassword"
            :placeholder="$t('demo.confirmPassword')"
            :rules="[
              { required: true, message: this.$msgUtil.get('E_0006'), trigger: 'onChange' },
              { validator: confirmPassword, message: this.$msgUtil.get('E_0009'), trigger: 'onChange' }
            ]"
          />
          <van-field v-model="loginForm.testField" :rules="rules.testField" />
          <van-button style="margin:10px 0 150px 5px" @click="submit()">提交</van-button>
          <van-button style="margin:10px 0 150px 20px" @click="cancel()">删除验证</van-button>
        </van-form>
      </div>
    </div>
  </div>
</template>

<script>
import { getToken } from '@/utils/auth'
import { validEmail, validEnglishOrNum, validateServerError, validateServerErrorMessage } from '@/utils/validate'
export default {
  components: {},
  data() {
    const vantValidEmail = validEmail
    // 验证两次输入的邮箱是否一致
    const confirmEmail = () => {
      return this.loginForm.confirmMailAddress === this.loginForm.mailAddress
    }
    // 验证密码长度
    const checkpwdLength = val => {
      return /^.{6,12}$/.test(val)
    }
    // 验证两次密码是否一致
    const confirmPassword = () => {
      return this.loginForm.confirmPassword === this.loginForm.password
    }
    // 验证密码是否位英文或数字
    // const vantPasswordCheck = val => {
    //   return /^[A-Za-z0-9]+$/.test(val)
    // }
    const vantPasswordCheck = validEnglishOrNum
    return {
      serverErrors: [],
      serverErrorMessages: {},
      vantValidEmail,
      checkpwdLength,
      confirmEmail,
      confirmPassword,
      showLogin: true,
      messageContent: '',
      vantPasswordCheck,
      persons: [],
      lang: 'zh',
      loginForm: {
        userName: '',
        mailAddress: '',
        confirmMailAddress: '',
        password: '',
        confirmPassword: '',
        year: '',
        testField: '',
        month: ''
      },
      rules: {
        testField: [],
        password: [
          { required: true, message: this.$msgUtil.get('E_0010'), trigger: 'onChange' }
        ]
      }
    }
  },
  created() {
    var token = getToken()
    if (token) {
      this.showLogin = false
    }
  },
  methods: {
    loginAgent() {},
    login() {
      this.$router.push({ path: '/login' })
    },
    changeLang() {
      this.$i18n.locale = this.lang
    },
    getListData() {
      const thisObj = this
      thisObj
        .$request({
          url: '/demo/person/list',
          method: 'post',
          data: {}
        })
        .then(function(response) {
          thisObj.persons = response.dataList
        })
    },
    agentRegister() {
      this.$router.push({ path: '/agentRegister' })
    },
    getMessageContent() {
      this.messageContent = this.$msgUtil.get('demo', 'demo.getMessageContent')
    },
    showMessage() {
      this.messageContent = this.$msgUtil.message('demo', 'demo.showMessage')
    },
    showMessageBox() {
      this.messageContent = this.$msgUtil.messageBox('demo', 'demo.showMessageBox')
    },
    submit() {
      this.serverErrors = [{ msgCode: 'E_0011test', msgItemId: 'testField' }]
      this.rules.testField.push({
        serverFlag: true,
        validator: validateServerError.bind(this, 'testField'),
        message: validateServerErrorMessage(this, 'testField'),
        trigger: 'onChange'
      })
    },
    cancel() {
      // 全局方法去掉动态追加的消息验证
      this.validRemove(this)
    }
  }
}
</script>

<style lang="scss"></style>
