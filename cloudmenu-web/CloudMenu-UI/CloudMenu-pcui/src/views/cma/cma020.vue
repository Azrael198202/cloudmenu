<template>
  <div class="background">
    <div class="title">メニュー名:{{ menuName }}</div>
    <div class="body">
      <div class="left">
        <div>＜メニュー分類＞</div>
        <el-tabs v-model="active" @tab-click="handleClick">
          <el-tab-pane label="単品" name="1">
            <el-row>
              <el-row class="food-list" v-for="(item, index) in classificationList" :key="index">
                <el-col :span="12" class="img">
                  <img :src="item.url" />
                </el-col>
                <el-col :span="12" class="price">
                  <span>{{ index + 1 }}</span>
                  <span>{{ item.foodName }}</span>
                  <span>{{ item.foodNameJap }}</span>
                  <span>{{ item.foodprice }}</span>
                </el-col>
              </el-row>
            </el-row>
          </el-tab-pane>
          <el-tab-pane label="ランチ" name="2">
            <el-row>
              <el-row class="food-list" v-for="(item, index) in classificationList" :key="index">
                <el-col :span="12" class="img">
                  <img :src="item.url" />
                </el-col>
                <el-col :span="12" class="price">
                  <span>{{ index + 1 }}</span>
                  <span>{{ item.foodName }}</span>
                  <span>{{ item.foodNameJap }}</span>
                  <span>{{ item.foodprice }}</span>
                </el-col>
              </el-row>
            </el-row>
          </el-tab-pane>
          <el-tab-pane label="定食" name="3">
            <el-row>
              <el-row class="food-list" v-for="(item, index) in classificationList" :key="index">
                <el-col :span="12" class="img">
                  <img :src="item.url" />
                </el-col>
                <el-col :span="12" class="price">
                  <span>{{ index + 1 }}</span>
                  <span>{{ item.foodName }}</span>
                  <span>{{ item.foodNameJap }}</span>
                  <span>{{ item.foodprice }}</span>
                </el-col>
              </el-row>
            </el-row>
          </el-tab-pane>
          <el-tab-pane label="飲み物" name="4">
            <el-row>
              <el-row class="food-list" v-for="(item, index) in classificationList" :key="index">
                <el-col :span="12" class="img">
                  <img :src="item.url" />
                </el-col>
                <el-col :span="12" class="price">
                  <span>{{ index + 1 }}</span>
                  <span>{{ item.foodName }}</span>
                  <span>{{ item.foodNameJap }}</span>
                  <span>{{ item.foodprice }}</span>
                </el-col>
              </el-row>
            </el-row>
          </el-tab-pane>
          <el-tab-pane label="春限定" name="5">
            <el-row>
              <el-row class="food-list" v-for="(item, index) in classificationList" :key="index">
                <el-col :span="12" class="img">
                  <img :src="item.url" />
                </el-col>
                <el-col :span="12" class="price">
                  <span>{{ index + 1 }}</span>
                  <span>{{ item.foodName }}</span>
                  <span>{{ item.foodNameJap }}</span>
                  <span>{{ item.foodprice }}</span>
                </el-col>
              </el-row>
            </el-row>
          </el-tab-pane>
          <el-tab-pane label="分類" name="6">
            <el-row>
              <el-row class="food-list" v-for="(item, index) in classificationList" :key="index">
                <el-col :span="12" class="img">
                  <img :src="item.url" />
                </el-col>
                <el-col :span="12" class="price">
                  <span>{{ index + 1 }}</span>
                  <span>{{ item.foodName }}</span>
                  <span>{{ item.foodNameJap }}</span>
                  <span>{{ item.foodprice }}</span>
                </el-col>
              </el-row>
            </el-row>
          </el-tab-pane>
        </el-tabs>
      </div>

      <div class="right">
        <div>＜商品＞</div>
        <el-form ref="form" :model="form">
          <el-row>
            <el-col :span="5"><el-tag size="medium" type="success">商品名</el-tag></el-col>
            <el-col :span="15">
              <el-form-item>
                <el-input v-model="form.name"></el-input>
              </el-form-item>
            </el-col>
            <el-col :span="4">
              <el-button @click="changeView">表示切替</el-button>
            </el-col>
          </el-row>
          <el-row>
            <el-col :span="5">
              <div>
                <el-col><el-tag type="success">商品カテゴリ(大)</el-tag></el-col>
              </div>
              <el-form-item>
                <el-select v-model="value" placeholder="">
                  <el-option
                    v-for="item in bigFoodList"
                    :key="item.value"
                    :label="item.label"
                    :value="item.value"
                  ></el-option>
                </el-select>
              </el-form-item>
            </el-col>
            <el-col :span="5" :offset="2">
              <div>
                <el-tag type="success">商品カテゴリ(中)</el-tag>
              </div>
              <el-form-item>
                <el-select v-model="value" placeholder="">
                  <el-option
                    v-for="item in mediateFoodList"
                    :key="item.value"
                    :label="item.label"
                    :value="item.value"
                  ></el-option>
                </el-select>
              </el-form-item>
            </el-col>
            <el-col :span="5" :offset="2">
              <div>
                <el-tag type="success">商品カテゴリ(小)</el-tag>
              </div>
              <el-form-item>
                <el-select v-model="value" placeholder="">
                  <el-option
                    v-for="item in smallFoodList"
                    :key="item.value"
                    :label="item.label"
                    :value="item.value"
                  ></el-option>
                </el-select>
              </el-form-item>
            </el-col>
            <el-col :span="5">
              <div>
                <el-button type="danger">検索</el-button>
              </div>
            </el-col>
          </el-row>
        </el-form>
        <el-checkbox-group v-model="checkList">
          <el-row v-if="showfoodCard">
            <el-col :span="8" v-for="(item, index) in foodList" :key="index">
              <el-checkbox :label="index">
                <div class="card">
                  <div>{{ item.foodName }}</div>
                  <div>{{ item.foodprice }}</div>
                  <el-image style="width: 90px; height: 100px" :src="item.url"></el-image>
                </div>
              </el-checkbox>
            </el-col>
          </el-row>

          <el-row class="foodCollection" v-if="showfoodList">
            <el-row class="food-list" v-for="(item, index) in foodList" :key="index">
              <el-checkbox :label="index">
                <el-col :span="12" class="img">
                  <img :src="item.url" />
                </el-col>
                <el-col :span="12" class="price">
                  <span>{{ index + 1 }}</span>
                  <span>{{ item.foodName }}</span>
                  <span>{{ item.foodprice }}</span>
                </el-col>
              </el-checkbox>
            </el-row>
          </el-row>
        </el-checkbox-group>

        <el-row>
          <el-col :span="4" :offset="20">
            <el-button @click="reflect" type="danger">メニュー反映</el-button>
          </el-col>
        </el-row>
      </div>
    </div>
  </div>
</template>
<script>
export default {
  name: 'cmaMenuManage',
  data() {
    return {
      active: '6',
      mediateFoodList: [
        { label: '', value: '1' },
        { label: '', value: '2' }
      ],
      bigFoodList: [
        { label: '', value: '3' },
        { label: '', value: '4' }
      ],
      smallFoodList: [
        { label: '', value: '5' },
        { label: '', value: '6' }
      ],
      foodList: [
        {
          foodName: '青椒肉絲',
          foodprice: '￥1,200',
          url: 'https://img1.baidu.com/it/u=3371203006,329945664&fm=26&fmt=auto&gp=0.jpg'
        },
        {
          foodName: '回鍋肉',
          foodprice: '￥1,000',
          url: 'https://img1.baidu.com/it/u=3371203006,329945664&fm=26&fmt=auto&gp=0.jpg'
        },
        {
          foodName: 'エビチリ',
          foodprice: '￥900',
          url: 'https://img1.baidu.com/it/u=3371203006,329945664&fm=26&fmt=auto&gp=0.jpg'
        },
        {
          foodName: '水煮鱼',
          foodprice: '￥1,200',
          url: 'https://img1.baidu.com/it/u=3371203006,329945664&fm=26&fmt=auto&gp=0.jpg'
        },
        {
          foodName: '青島ビール',
          foodprice: '￥400',
          url: 'https://img1.baidu.com/it/u=3371203006,329945664&fm=26&fmt=auto&gp=0.jpg'
        },
        {
          foodName: '烏龍茶',
          foodprice: '￥400',
          url: 'https://img1.baidu.com/it/u=3371203006,329945664&fm=26&fmt=auto&gp=0.jpg'
        }
      ],
      checkList: [],
      showfoodCard: true,
      showfoodList: false,
      value: '',
      menuName: '春メニュー2021',
      form: { name: '' },
      classificationList: [
        {
          url: 'https://img2.baidu.com/it/u=1630259334,1984138298&fm=26&fmt=auto&gp=0.jpg',
          foodName: '青椒肉丝',
          foodNameJap: 'ピーマンと豚肉細切炒め',
          foodprice: '￥1,200'
        },
        {
          url: 'https://img2.baidu.com/it/u=1630259334,1984138298&fm=26&fmt=auto&gp=0.jpg',
          foodName: '麻婆豆腐',
          foodNameJap: 'マーボー豆腐',
          foodprice: '￥1,000'
        },
        {
          url: 'https://img2.baidu.com/it/u=1630259334,1984138298&fm=26&fmt=auto&gp=0.jpg',
          foodName: '泡椒牛蛙',
          foodNameJap: 'ウシガエルとビーフン山椒スープ煮物',
          foodprice: '￥900'
        }
      ]
    }
  },
  created() {
    var menuName = this.$route.query.menuName
    if (menuName) {
      this.menuName = menuName
    }
  },
  methods: {
    handleClick() {},
    reflect() {
      if (this.checkList.length) {
        for (var i = 0; i < this.checkList.length; i++) {
          this.classificationList.push(this.foodList[this.checkList[i]])
        }
      }
    },
    changeView() {
      this.showfoodCard = !this.showfoodCard
      this.showfoodList = !this.showfoodList
    }
  }
}
</script>
<style scoped>
.left {
  width: 35%;
}
.background {
  margin: 20px;
}
.body {
  display: flex;
}
.bigFont {
  font-size: 32px;
}
</style>
<style lang="scss">
.el-tag--medium {
  height: 36px;
  line-height: 36px;
}

.el-checkbox__input {
  margin-bottom: 100px;
}
.el-checkbox__inner {
  margin-bottom: 200px;
}
.el-checkbox {
  margin-bottom: -80px;
}
</style>
