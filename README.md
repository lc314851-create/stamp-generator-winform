# 印章生成器 - Stamp Generator

一个简洁好用的 Windows 桌面印章图片生成工具，基于 WinForm + .NET 8 开发。

## 功能特点

- 📝 **自定义文字** - 输入任意印章内容
- 🔴 **多种颜色** - 支持红色、蓝色、绿色
- ⭕ **多种形状** - 圆形、方形、椭圆
- 📐 **灵活排版** - 支持横排和竖排
- 🔍 **精细调节** - 字号、间距、边距、旋转角度
- 🎯 **一键置中** - 快速重置为最佳效果
- 💾 **图片导出** - 一键保存 PNG 格式图片

## 系统要求

- Windows 10/11
- .NET 8 Runtime

## 运行方式

### 方式一：编译运行

```bash
# 克隆项目
git clone https://github.com/lc314851-create/stamp-generator-winform.git
cd stamp-generator-winform

# 编译运行
dotnet run
```

### 方式二：发布为可执行文件

```bash
dotnet publish -c Release -r win-x64 --self-contained false -o ./publish
```

### 方式三：发布为独立 exe

```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o ./publish
```

## 项目结构

```
stamp-generator-winform/
├── Program.cs           # 程序入口
├── MainForm.cs          # 主窗体逻辑
├── StampGenerator.csproj # 项目文件
└── README.md            # 说明文档
```

## 技术栈

- .NET 8
- Windows Forms (WinForm)
- System.Drawing

## 截图预览

界面简洁直观，左侧控制面板，右侧实时预览。

## 使用说明

1. 输入印章文字
2. 选择印章形状和颜色
3. 调整参数（字号、间距、边距、旋转）
4. 实时预览效果
5. 点击「下载图片」保存 PNG 文件

## License

MIT
