# DotNetUtils.Core
.net Core 公共工具类

此工具类库是基于.net core 2.2封装的，里面包括：阿里oss帮助类，图片帮助类，文件帮助类，下载类，随机数帮助类，对象映射帮助类，字符串帮助类，验证码生成，时间帮助类，中国日历帮助类，任务下发帮助类等，后面将持续更新中！

文档说明见：HTTPS：//blog.csdn.net/agoling/article/details/87068943

"BaseResult"类前后端接口返回参命名规范

一、命名规范
小驼峰法命名：除第一个单词之外，其他单词首字母大写
1、方法
    获取单个对象的方法用get作为前缀。
    获取多个对象方法用search作为前缀。
    删除的方法用delete作为前缀。
    (添加+修改)用save作为前缀。
2、变量
变量取名应见名识意,尽量用英文单词,而不是缩写.

example：pageIndex, pageSize

二、接口返回格式
{
    code:200,       //数字
    msg:"success",  //成功时为"success"，错误时则是错误信息
    result:{},        //对象
}

三、操作类返回格式
增删改都返回对应ID
查询返回完整模型
{
    code:200,       //数字
    msg:"success", //字符串
    result:{},
}

四、列表接口基础出入参
请求的基础参数
{
    keyWord:"",    //关键词搜索
    pageIndex:1,   //当前页码
    pageSize:10,   //分页数量
}
返回的基础内容
{
    code:200, 
    msg:"success",
    result:{
        list:[{},{}],  //返回的列表
        pageIndex:1,   //当前页码
        pageSize:10,   //分页数量
        totalCount:100,//总条数
        totalPages:10  //总页数 
        
    },
}

五、错误码
返回码	标识	说明
200	success	请求成功
999	defaultError	系统繁忙，此时请开发者稍候再试
3000	nullData	未找到数据
4000	notLogin	未登录
5000	exception	异常
5010	dataIsValid	数据验证不通过
6000	dataExpire	数据过期
7000	businessError	默认业务性异常

六、YAPI编写规范
请求参数
Headers
参数名称	类型	是否必须	示例	备注
token	string	必须	'6fc7ab0a1a1a42639599233f65c74ccf'	token
Body
参数名称	类型	是否必须	示例	备注
keyWord	string	必须	'测试'	关键词搜索
pageIndex	int	必须	1	当前页码
pageSize	int	必须	10	分页数量
返回参数
名称	类型	是否必须	默认值	备注
code	int	必须	200	返回状态码
msg	string	必须	success	成功时为"success"，错误时则是错误信息
result	object	必须	{}	返回的数据
