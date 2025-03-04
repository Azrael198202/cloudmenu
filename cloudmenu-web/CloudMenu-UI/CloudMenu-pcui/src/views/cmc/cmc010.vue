<template>
  <div class="container">
    <div class="main-top">
      <div class="main">
        <div class="fl">クラウドメニュー</div>
        <div class="fr"><i class="fa fa-question-circle" aria-hidden="true" @click="help" /></div>
      </div>
    </div>
    <div class="main">
      <div class="main-in">
        <ul>
          <li
            v-for="item in menuList"
            :key="item.menuId"
            :style="{ left: item.menuXPosition+'px', top: item.menuYPosition+'px' }"
            @click="toLink(item.menuLink, item.menuUnusedFlg)"
          >
            <div :style="{ background: item.menuButtonColor, color: item.menuFontColor }">
              <i class="fa" :class="item.menuButtonIcon" aria-hidden="true" />
              <span>{{ item.menuName }}</span>
            </div>
          </li>
        </ul>
      </div>
    </div>
  </div>
</template>

<script>
import { searchSystemMenu } from '@/api/cmc/cmc010'
export default {
  data() {
    return {
      menuList: [{
        menuYPosition: 0,
        menuXPosition: 0,
        menuId: '',
        menuName: '',
        menuLink: '',
        menuButtonColor: '',
        menuButtonIcon: '',
        menuFontColor: '',
        menuUnusedFlg: ''
      }]
    }
  },
  created() {
    const useClassify = '010' // 使用者分类，010（工作人员）
    this.searchSystemMenu({ menuUserKbn: useClassify })
  },
  methods: {
    // 获取菜单列表信息
    searchSystemMenu(useClassify) {
      const thisObj = this
      searchSystemMenu(useClassify).then(response => {
        if (response.status === 200) {
          thisObj.menuList = response.menuList
        } else if (response.status === 601) {
          thisObj.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
        } else if (response.status === 602) {
          thisObj.$msgUtil.messageNew(response.msgCode, response.msgInfo, this)
        }
      })
    },

    // 跳转
    toLink(linkUrl, flg) {
      if (flg === '0') {
        this.$router.push({
          path: linkUrl,
          query: this.otherQuery
        })
      }
    },
    // 帮助PDF显示
    help() {
      // 店员的pdf帮助说明
      window.open(
        'https://www.sato.co.jp/support/printertool/tool/' +
            'multi-labelist-component/pdf/MLCMP-Manual-Reference-v5_9_5_0_r210121.pdf'
      )
    }
  }
}
</script>
<style lang="scss" scoped>
@import '@/styles/variables.scss';
.container {
  height: 100%;
  background: #f5f5f5;
}

.main-top {
  height: 30px;
  background: $white;
  line-height: 30px;

  .main {
    padding: 0;
    font-size: 16px;

    .fr {
      i {
        font-size: 16px;
        cursor: pointer;
      }
    }
  }
}

.main-in {
  width: 580px;
  margin: 0 auto;
  height: auto;

  ul {
    position: relative;
  }

  li {
    width: 25%;
    min-height: 90px;
    float: left;
    margin-bottom: 14px;
    cursor: pointer;
    position: absolute;
    left: 0;
    top: 0;

    div {
      width: 135px;
      height: 98px;
      background: $menuBtnColor;
      border-radius: 3px;
      display: flex;
      flex-direction: column;
      justify-content: center;
      align-items: center;
      color: $white;

      i {
        font-size: 38px;
      }

      span {
        display: block;
        margin-top: 8px;
      }
    }
  }

  li:nth-child(4n) {
    margin-right: 0;
  }
}
</style>
