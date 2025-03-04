module.exports = [{
        url: '/zaiko/searchNyukoList.do',
        type: 'post',
        response: _ => {
            return {
                status: 200,
                searchList: [{
                        categoryKbn: '010',
                        categoryName: '商品',
                        targetNumber: 10,
                        targetName: '鸡肉',
                        storingQuantity: 5,
                        unitName: '斤'
                    },
                    {
                        categoryKbn: '1',
                        categoryName: '特殊商品',
                        targetNumber: 20,
                        targetName: '牛肉XXXXXXXXXXX',
                        storingQuantity: 5,
                        unitName: '斤'
                    },
                    {
                        categoryKbn: '1',
                        categoryName: '商品',
                        targetNumber: 30,
                        targetName: '猪肉',
                        storingQuantity: 3,
                        unitName: '斤'
                    }
                ],
                msgList: [{ msgCode: 'E_00010', msgOption: '入庫検索', msgItemId: '' }]
            }
        }
    },
    {
        url: '/checkNyukoDate.do',
        type: 'post',
        response: _ => {
            return {
                status: 200,
                checkInfo: {
                    date: '2020/06/04',
                    stored: false
                }
            }
        }
    }
]