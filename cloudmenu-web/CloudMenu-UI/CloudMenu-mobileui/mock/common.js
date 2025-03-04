module.exports = [{
    url: '/order/searchKubun.do',
    type: 'post',
    response: _ => {
        return {
            status: 200,
            categoryList: [{
                    categoryKbn: '001',
                    categoryKbnName: '燕翅鲍',
                    categoryKbnAbbr: ''
                },
                {
                    categoryKbn: '002',
                    categoryKbnName: '煎菜',
                    categoryKbnAbbr: ''
                },
                {
                    categoryKbn: '003',
                    categoryKbnName: '海鲜 老上海',
                    categoryKbnAbbr: ''
                },
                {
                    categoryKbn: '004',
                    categoryKbnName: '家常小炒',
                    categoryKbnAbbr: ''
                },
                {
                    categoryKbn: '005',
                    categoryKbnName: '野菜 豆腐',
                    categoryKbnAbbr: ''
                },
                {
                    categoryKbn: '006',
                    categoryKbnName: '羹',
                    categoryKbnAbbr: ''
                },
                {
                    categoryKbn: '007',
                    categoryKbnName: '炒饭',
                    categoryKbnAbbr: ''
                },
                {
                    categoryKbn: '008',
                    categoryKbnName: '甜点',
                    categoryKbnAbbr: ''
                },
                {
                    categoryKbn: '009',
                    categoryKbnName: '料理長...',
                    categoryKbnAbbr: ''
                }
            ],
            msgList: [{
                msgCode: '1',
                msgItemId: '1',
                msgOption: ''
            }]
        }
    }
}]