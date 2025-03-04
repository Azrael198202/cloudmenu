<template>
  <div class="container">
    <div class="main">
      <el-row type="flex" align="middle">
        <el-col :span="3"><span>商品カテゴリ(大)</span></el-col>
        <el-col :span="4">
          <el-select v-model="form.catalogBig">
            <el-option
              v-for="item in form.catalogBigList"
              :key="item.value"
              :label="item.label"
              :value="item.value"
            ></el-option>
          </el-select>
        </el-col>
        <el-col :span="3" :offset="1"><span>商品カテゴリ(中)</span></el-col>
        <el-col :span="4">
          <el-select v-model="form.catalogMid">
            <el-option
              v-for="item in form.catalogMidList"
              :key="item.value"
              :label="item.label"
              :value="item.value"
            ></el-option>
          </el-select>
        </el-col>
        <el-col :span="3" :offset="1"><span>商品カテゴリ(小)</span></el-col>
        <el-col :span="4">
          <el-select v-model="form.catalogMini">
            <el-option
              v-for="item in form.catalogMiniList"
              :key="item.value"
              :label="item.label"
              :value="item.value"
            ></el-option>
          </el-select>
        </el-col>
      </el-row>
      <el-row type="flex" align="middle">
        <el-col :span="3"><span>商品名</span></el-col>
        <el-col :span="4">
          <el-input v-model="form.monoName"></el-input>
        </el-col>
        <el-col :span="3" :offset="1"><span>価格(税込)</span></el-col>
        <el-col :span="4">
          <el-input v-model="form.traffic"></el-input>
        </el-col>
      </el-row>
      <el-row type="flex" align="middle">
        <el-col :span="3"><span>付加情報</span></el-col>
        <el-col :span="3">
          <el-checkbox v-model="form.recommendFlg">お薦め</el-checkbox>
        </el-col>
        <el-col :span="3">
          <el-checkbox v-model="form.timeZone">提供時間帯</el-checkbox>
        </el-col>
        <el-col :span="3">
          <el-time-picker arrow-control v-model="form.value1" format="HH:MM"></el-time-picker>
        </el-col>
        <el-col :span="1" :offset="1" class="text-center">～</el-col>
        <el-col :span="3">
          <el-time-picker arrow-control v-model="form.value2" format="HH:MM"></el-time-picker>
        </el-col>
      </el-row>
      <el-row type="flex" align="middle">
        <el-col :span="3"><span>カロリー</span></el-col>
        <el-col :span="4">
          <el-input type="text" v-model="form.kcal" maxlength="10">
            <i class="el-input__icon" slot="suffix">Kcal</i>
          </el-input>
        </el-col>
        <el-col :span="3" :offset="1"><span>調理時間目安</span></el-col>
        <el-col :span="4">
          <el-input v-model="form.makeTime">
            <i class="el-input__icon" slot="suffix">分</i>
          </el-input>
        </el-col>
      </el-row>
      <el-row type="flex" align="middle">
        <el-col :span="3"><span>アレルギー食材</span></el-col>
      </el-row>
      <el-row type="flex" align="middle">
        <el-col :span="24">
          <el-checkbox-group v-model="form.checkedShokuzaiList">
            <el-checkbox v-for="shokuzai in form.shokuzaiList" :label="shokuzai" :key="shokuzai">
              {{ shokuzai }}
            </el-checkbox>
          </el-checkbox-group>
        </el-col>
      </el-row>
      <el-row type="flex" align="middle">
        <el-col :span="3"><span>画像</span></el-col>
        <el-col :span="3" :offset="3"><span>商品紹介</span></el-col>
      </el-row>
      <el-row type="flex" align="middle">
        <el-col :span="4" :offset="1">
          <div class="block">
            <el-image style="width: 100px; height: 100px" :src="form.url" :preview-src-list="form.srcList">
              <div slot="error" class="el-image__error"></div>
            </el-image>
          </div>
        </el-col>
        <el-col :span="17" :offset="2">
          <el-input type="textarea" v-model="form.textarea" rows="5" resize="none"></el-input>
        </el-col>
      </el-row>
      <el-row type="flex" align="middle">
        <el-col :span="4" :offset="1">
          <el-button type="primary" plain>参照</el-button>
        </el-col>
      </el-row>
      <el-row type="flex" align="middle">
        <el-col :span="4" :offset="23">
          <el-button type="primary" plain>登録</el-button>
        </el-col>
      </el-row>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      form: {
        catalogBig: '  ',
        catalogBigList: [
          {
            value: 'B1',
            label: '単品'
          },
          {
            value: 'B2',
            label: '定食'
          },
          {
            value: 'B3',
            label: 'コース料理'
          }
        ],
        catalogMid: '  ',
        catalogMidList: [
          {
            value: 'M1',
            label: '肉料理'
          },
          {
            value: 'M2',
            label: '魚料理'
          },
          {
            value: 'M3',
            label: '飲み物'
          },
          {
            value: 'M4',
            label: 'ランチ'
          },
          {
            value: 'M5',
            label: 'ディナー'
          }
        ],
        catalogMini: '  ',
        catalogMiniList: [
          {
            value: 'N1',
            label: '青椒肉絲'
          },
          {
            value: 'N2',
            label: '回鍋肉'
          },
          {
            value: 'N3',
            label: '水煮魚'
          },
          {
            value: 'N4',
            label: '青島ビール'
          },
          {
            value: 'N5',
            label: '烏龍茶'
          },
          {
            value: 'N6',
            label: '中華丼セット'
          },
          {
            value: 'N7',
            label: '皇帝ディナー'
          }
        ],
        checkedShokuzaiList: ['鶏卵', '牛乳'],
        shokuzaiList: [
          '鶏卵',
          '牛乳',
          '小麦',
          'ピーナツ',
          '果物類',
          '魚卵',
          '甲殻類',
          'ナッツ類',
          'ソバ',
          '魚類',
          'その他'
        ],
        monoName: '',
        traffic: '',
        recommendFlg: true,
        timeZone: false,
        kcal: '',
        makeTime: '',
        url: require('../../assets/images/cma040.jpg'),
        srcList: [require('../../assets/images/cma040.jpg')]
      },
      methods: {
        // handleCheckAllChange(val) {
        //   this.checkedShokuzaiList = val ? cityOptions : []
        //   this.isIndeterminate = false
        // },
        // handleCheckedCitiesChange(value) {
        //   let checkedCount = value.length
        //   this.checkAll = checkedCount === this.cities.length
        //   this.isIndeterminate = checkedCount > 0 && checkedCount < this.cities.length
        // }
      }
    }
  }
}
</script>
<style>
span {
  font-size: 14px;
}
.el-row {
  margin-top: 10px !important;
}
/* 时间空间宽度 */
.el-date-editor--time {
  width: 160px !important;
}
</style>
