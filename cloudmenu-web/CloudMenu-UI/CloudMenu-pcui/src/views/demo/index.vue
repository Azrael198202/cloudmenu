<template>
  <div class="dashboard-container">
    演示功能
    <el-row class="login-agent">
      <el-col :span="2">
        <el-button type="primary" @click="cmaMenuList">メニュー一覧</el-button>
      </el-col>
      <el-col :span="2">
        <el-button type="primary" @click="cmaMenuManage">メニュー登録</el-button>
      </el-col>
      <el-col :span="2">
        <el-button type="primary" @click="cmaGoodsList">商品一覧</el-button>
      </el-col>
      <el-col :span="2">
        <el-button type="primary" @click="cmaGoodsUp">商品登録</el-button>
      </el-col>
      <el-col :span="2">
        <el-button type="primary" @click="cmaTenpoInfoToroku">店舗情報登録</el-button>
      </el-col>
      <el-col :span="2">
        <el-button type="primary" @click="cmaTableToroku">座席登録</el-button>
      </el-col>
      <el-col :span="2">
        <el-button type="primary" @click="cmaMaster">区分マスタ登録</el-button>
      </el-col>
    </el-row>
    <el-row class="login-agent">
      <el-col :span="2">
        <el-button type="primary" @click="cmaTableUseInfoList">座席状況一覧</el-button>
      </el-col>
      <el-col :span="2">
        <el-button type="primary" @click="cmaKaikeiToroku">会計登録</el-button>
      </el-col>
      <el-col :span="2">
        <el-button type="primary" @click="cmaZaikoItiran">在庫一覧</el-button>
      </el-col>
      <el-col :span="2">
        <el-button type="primary" @click="cmaNyukoItiran">入庫一覧</el-button>
      </el-col>
      <el-col :span="2">
        <el-button type="primary" @click="cmcStaffList">スタッフトップ</el-button>
      </el-col>
    </el-row>
    <el-card class="box-card">
      <div slot="header" class="clearfix">
        <span>多语言</span>
      </div>
      <div style="margin-top: 20px">
        <el-radio-group ref="lang" v-model="lang" size="mini" @change="changeLang">
          <el-radio-button label="zh">中文</el-radio-button>
          <el-radio-button label="ja">日本語</el-radio-button>
        </el-radio-group>
      </div>
    </el-card>
    <el-card class="box-card">
      <div slot="header" class="clearfix">
        <span>icons</span>
      </div>
      <div style="margin-top: 20px">
        <i class="fa fa-hand-rock-o" />
        <br />
        <i class="required fa fa-asterisk" aria-hidden="true" />
        <br />
        <el-button type="success" class="fa fa-hand-rock-o" circle />
      </div>
    </el-card>

    <el-card class="box-card">
      <div slot="header" class="clearfix">
        <span>Mock数据</span>
      </div>
      <div style="margin-top: 20px">
        <el-button type="success" class="fa fa-hand-rock-o" @click="getListData">获取mock数据</el-button>
        <div v-for="(person, index) in persons" :key="index">{{ person.name }}</div>
      </div>
    </el-card>

    <el-card class="box-card">
      <div slot="header" class="clearfix">
        <span>消息提示</span>
      </div>
      <div style="margin-top: 20px">
        <el-button type="success" class="fa fa-hand-rock-o" @click="getMessageContent">获取消息内容</el-button>
        {{ messageContent }}
      </div>
      <div style="margin-top: 20px">
        <el-button type="success" class="fa fa-hand-rock-o" @click="showMessage">弹出消息提示</el-button>
      </div>
      <div style="margin-top: 20px">
        <el-button type="success" class="fa fa-hand-rock-o" @click="showMessageBox">弹出消息框</el-button>
      </div>
    </el-card>

    <el-card class="box-card">
      <div slot="header" class="clearfix">
        <span>表单验证</span>
      </div>
      <div style="margin-top: 20px">
        <el-form ref="loginForm" :rules="rules" :model="loginForm">
          <el-form-item prop="userName">
            <el-input
              v-model="loginForm.userName"
              class="required"
              suffix-icon="fa fa-asterisk"
              :placeholder="$t('P0108.userName')"
            >
              <i class="fa fa-asterisk" />
            </el-input>
          </el-form-item>
          <el-form-item prop="mailAddress">
            <el-input
              v-model="loginForm.mailAddress"
              prefix-icon="fa fa-asterisk"
              :placeholder="$t('P0108.mailAddress')"
            />
          </el-form-item>
          <el-form-item prop="confirmMailAddress">
            <el-input v-model="loginForm.confirmMailAddress" :placeholder="$t('P0108.confirmMailAddress')" />
          </el-form-item>
          <el-form-item prop="password">
            <el-input v-model="loginForm.password" :placeholder="$t('P0108.password')" />
          </el-form-item>
          <el-form-item prop="confirmPassword">
            <el-input v-model="loginForm.confirmPassword" :placeholder="$t('P0108.confirmPassword')">
              <i slot="suffix" class="fa fa-eye" aria-hidden="true" />
            </el-input>
          </el-form-item>

          <el-form-item prop="yearMonth">
            <div style="float:left">年月</div>
            <div style="float:left">
              <el-input v-model="loginForm.year" style="width:120px" />
              <el-input v-model="loginForm.month" style="width:120px" />
            </div>
          </el-form-item>
          <el-button class="button-position" @click="submit()">提交</el-button>
        </el-form>
      </div>
    </el-card>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
import { validEnglishOrNum, validateServerError } from '@/utils/validate.js'

export default {
  name: 'DemoPage',
  components: {},
  data() {
    // 多个项目一行的情况下，尽量的合并显示错误内容
    const validYearMonth = (rule, value, callback) => {
      if (
        !(this.loginForm.year && this.loginForm.year.trim().length > 0) &&
        !(this.loginForm.month && this.loginForm.month.trim().length > 0)
      ) {
        // 允许为空
        // 校验通过
        callback()
      }
      if (this.loginForm.year.length != 4 || this.loginForm.month.length != 2) {
        callback(new Error('ddd ddd ddd'))
      } else {
        // 校验通过
        callback()
      }
    }
    return {
      lang: 'zh',
      persons: [],
      messageContent: '',
      serverErrors: [],
      rules: {
        userName: [
          {
            required: true,
            message: this.$msgUtil.get('E000001', '用户名'),
            trigger: 'change'
          },
          { validator: validEnglishOrNum, trigger: 'change' },
          {
            validator: validateServerError.bind(this)
          }
        ],
        mailAddress: [
          {
            required: true,
            message: this.$msgUtil.get('E000001', '邮箱地址'),
            trigger: 'change'
          },
          { validator: validEnglishOrNum, trigger: 'change' }
        ],
        confirmMailAddress: [
          {
            validator: validEnglishOrNum,
            trigger: 'blur'
          }
        ],
        password: [
          {
            required: true,
            message: this.$msgUtil.get('E000001', '密码'),
            trigger: 'change'
          },
          { validator: validEnglishOrNum, trigger: 'change' }
        ],
        confirmPassword: [
          {
            validator: validEnglishOrNum,
            trigger: 'blur'
          }
        ],
        yearMonth: [
          {
            validator: validYearMonth,
            trigger: 'blur'
          }
        ]
      },

      loginForm: {
        userName: '',
        mailAddress: '',
        confirmMailAddress: '',
        password: '',
        confirmPassword: '',
        year: '',
        month: ''
      },
      otherQuery: {}
    }
  },
  created() {},
  methods: {
    changeLang() {
      if (this.$refs.lang.value == 'zh') {
        this.$i18n.locale = 'zh-cn'
        window.localStorage.setItem('language', 'zh-cn')
      } else {
        this.$i18n.locale = 'ja-jp'
        window.localStorage.getItem('language', 'ja-jp')
      }
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
    getMessageContent() {
      this.messageContent = this.$msgUtil.get('E000001')
    },
    showMessage() {
      this.messageContent = this.$msgUtil.message('E000001', 'demo.userName')
    },
    showMessageBox() {
      this.messageContent = this.$msgUtil.messageBox('E000001', 'demo.userName')
    },
    submit() {
      const thisObj = this
      thisObj.serverErrors = []
      thisObj.$refs.loginForm.validate(valid => {
        if (valid) {
          thisObj
            .$request({
              url: '/demo/submittest',
              method: 'post',
              params: {}
            })
            .then(function(response) {
              if (response.status == 601) {
                thisObj.serverErrors = response.msgList
                thisObj.$refs.loginForm.validate()
              } else if (response.status == 602) {
                this.$msgUtil.message(response.msgList[0].msgCode)
              }
            })
        }
      })
    },

    cmaMenuList() {
      this.$router.push({
        path: '/cmaMenuList',
        query: this.otherQuery
      })
    },
    cmaMenuManage() {
      this.$router.push({
        path: '/cmaMenuManage',
        query: this.otherQuery
      })
    },
    cmaGoodsList() {
      this.$router.push({
        path: '/cmaGoodsList',
        query: this.otherQuery
      })
    },
    cmaGoodsUp() {
      this.$router.push({
        path: '/cmaGoodsUp',
        query: this.otherQuery
      })
    },
    cmaTenpoInfoToroku() {
      this.$router.push({
        path: '/cmaTenpoInfoToroku',
        query: this.otherQuery
      })
    },
    cmaTableToroku() {
      this.$router.push({
        path: '/cmaTableToroku',
        query: this.otherQuery
      })
    },
    cmaTableUseInfoList() {
      this.$router.push({
        path: '/cmaTableUseInfoList',
        query: this.otherQuery
      })
    },
    cmaKaikeiToroku() {
      this.$router.push({
        path: '/cmaKaikeiToroku',
        query: this.otherQuery
      })
    },
    cmaZaikoItiran() {
      this.$router.push({
        path: '/cmaZaikoItiran',
        query: this.otherQuery
      })
    },
    cmaNyukoItiran() {
      this.$router.push({
        path: '/cmaNyukoItiran',
        query: this.otherQuery
      })
    },
    cmcStaffList() {
      this.$router.push({
        path: '/cmcStaffList',
        query: this.otherQuery
      })
    },
    cmaMaster() {
      this.$router.push({
        path: '/cmaMaster',
        query: this.otherQuery
      })
    }
  }
}
</script>
<style>
.box-card {
  width: 480px;
  margin: 20px;
}

.login-agent {
  margin: 30px;
}
</style>
