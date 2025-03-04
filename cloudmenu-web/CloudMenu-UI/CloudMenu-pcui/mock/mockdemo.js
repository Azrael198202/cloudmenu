const tokens = {
  admin: {
    token: 'admin-token'
  },
  editor: {
    token: 'editor-token'
  }
}

const users = {
  'admin-token': {
    roles: ['admin'],
    introduction: 'I am a super administrator',
    avatar: 'https://wpimg.wallstcn.com/f778738c-e4f8-4870-b634-56703b4acafe.gif',
    name: 'Super Admin'
  },
  'editor-token': {
    roles: ['editor'],
    introduction: 'I am an editor',
    avatar: 'https://wpimg.wallstcn.com/f778738c-e4f8-4870-b634-56703b4acafe.gif',
    name: 'Normal Editor'
  }
}

module.exports = [
  //验证当有重复的用户名的check
  {
    url: '/demo/submittestError',
    type: 'post',
    response: _ => {
      return {
        status: 601,
        msgList: [{ msgCode: 'E000008', msgItemId: 'userName' }]
      }
    }
  },
  //验证当没有重复的用户名的check时
  {
    url: '/demo/submittestSuccess',
    type: 'post',
    response: _ => {
      // return {
      //   status: 601,
      //   msgList: [{ msgCode: 'E000008', msgItemId: 'memberUsername' }]
      // }
      return {
        status: 200
      }
    }
  },
  // user logout
  {
    url: '/demo/person/list',
    type: 'post',
    response: _ => {
      return {
        status: 200,
        'data-list': [
          { name: 'user1', age: 12, content: 'asdada' },
          { name: 'user1', age: 12, content: 'asdada' },
          { name: 'user1', age: 12, content: 'asdada' }
        ]
      }
    }
  },
  // user logout
  {
    url: '/demo/person/list',
    type: 'post',
    response: _ => {
      return {
        status: 200,
        dataList: [
          { name: 'user1', age: 12, content: 'asdada' },
          { name: 'user1', age: 12, content: 'asdada' },
          { name: 'user1', age: 12, content: 'asdada' }
        ]
      }
      return {
        status: 200,
        data: { token: 'token' }
      }
    }
  }
]
