<template>
  <div class="container">
    <div class="main">
      <el-row type="flex" align="middle">
        <el-col :span="3"><span>商品カテゴリ(大)</span></el-col>
        <el-col :span="4">
          <el-select v-model="catalogueBig">
            <el-option
              v-for="item in catalogBigList"
              :key="item.value"
              :label="item.label"
              :value="item.value"
            ></el-option>
          </el-select>
        </el-col>
        <el-col :span="3" :offset="1"><span>商品カテゴリ(中)</span></el-col>
        <el-col :span="4">
          <el-select v-model="catalogueMid">
            <el-option
              v-for="item in catalogMidList"
              :key="item.value"
              :label="item.label"
              :value="item.value"
            ></el-option>
          </el-select>
        </el-col>
        <el-col :span="3" :offset="1"><span>商品カテゴリ(小)</span></el-col>
        <el-col :span="4">
          <el-select v-model="catalogueMini">
            <el-option
              v-for="item in catalogMiniList"
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
          <el-input v-model="input"></el-input>
        </el-col>
        <el-col :span="2" :offset="15">
          <el-button>検索</el-button>
        </el-col>
      </el-row>
      <el-table
        ref="singleTable"
        border
        :data="tableData"
        highlight-current-row
        @current-change="handleCurrentChange"
        style="width: 100%;margin-top:15px"
        height="480"
      >
        <el-table-column type="index" label="No" width="100"></el-table-column>
        <el-table-column property="catalogueBig" label="商品カテゴリ(大)" width="170"></el-table-column>
        <el-table-column property="catalogueMid" label="商品カテゴリ(中)" width="170"></el-table-column>
        <el-table-column property="catalogueMini" label="商品カテゴリ(小)" width="180"></el-table-column>
        <el-table-column property="monoName" label="商品名" width="209"></el-table-column>
        <el-table-column property="tariff" label="価格(税込)" width="170"></el-table-column>
      </el-table>
      <div style="margin-top: 20px">
        <el-button>戻る</el-button>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      catalogueBig: ' ',
      catalogueMid: ' ',
      catalogueMini: ' ',
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
      tableData: [
        {
          catalogueBig: '単品',
          catalogueMid: '肉料理',
          catalogueMini: '牛肉',
          monoName: '青椒肉絲',
          tariff: '￥1200'
        },
        {
          catalogueBig: '単品',
          catalogueMid: '肉料理',
          catalogueMini: '豚肉',
          monoName: '回鍋肉',
          tariff: '￥1000'
        },
        {
          catalogueBig: '単品',
          catalogueMid: '魚料理',
          catalogueMini: '海鮮',
          monoName: 'エビチリ',
          tariff: '￥900'
        },
        {
          catalogueBig: '単品',
          catalogueMid: '魚料理',
          catalogueMini: '川魚',
          monoName: '水煮魚',
          tariff: '￥800'
        },
        {
          catalogueBig: '単品',
          catalogueMid: '飲み物',
          catalogueMini: 'アルコール',
          monoName: '青島ビール',
          tariff: '￥400'
        },
        {
          catalogueBig: '単品',
          catalogueMid: '飲み物',
          catalogueMini: 'アルコール',
          monoName: '紹興酒',
          tariff: '￥500'
        },
        {
          catalogueBig: '単品',
          catalogueMid: '飲み物',
          catalogueMini: 'ソフトドリンク',
          monoName: '烏龍茶',
          tariff: '￥300'
        },
        {
          catalogueBig: '定食',
          catalogueMid: 'ランチ',
          catalogueMini: '丼もの',
          monoName: '中華丼セット',
          tariff: '￥1200'
        },
        {
          catalogueBig: '定食',
          catalogueMid: 'ランチ',
          catalogueMini: '麺類',
          monoName: '拉麺、餃子セット',
          tariff: '￥1000'
        },
        {
          catalogueBig: 'コース料理',
          catalogueMid: 'ディナー',
          catalogueMini: '食事のみ',
          monoName: '皇帝ディナー',
          tariff: '￥4000'
        },
        {
          catalogueBig: 'コース料理',
          catalogueMid: 'ディナー',
          catalogueMini: '飲み放題付',
          monoName: '鳳凰ディナー',
          tariff: '￥1200'
        }
      ],
      currentRow: null
    }
  },

  methods: {
    setCurrent(row) {
      this.$refs.singleTable.setCurrentRow(row)
    },
    handleCurrentChange(val) {
      this.currentRow = val
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
</style>
