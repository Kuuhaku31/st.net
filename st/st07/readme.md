# Note

## 项目结构

`MainWindow.xaml` - 定义了 UI 界面，使用了数据绑定来连接 ViewModel。
`ViewModel.cs` - 实现了 ViewModel 类，包含了属性和命令逻辑。

---

| 简写 | 全称                  | 中文               | 简介                                                                                                                   |
| ---- | --------------------- | ------------------ | ---------------------------------------------------------------------------------------------------------------------- |
| MVC  | Model-View-Controller | 模型-视图-控制器   | 将应用程序分为三个核心组件：模型（处理数据和业务逻辑）、视图（负责显示数据）和控制器（处理用户输入并协调模型和视图）。 |
| MVP  | Model-View-Presenter  | 模型-视图-演示者   | 类似于MVC，但引入了一个演示者（Presenter）来处理视图和模型之间的交互，视图只负责显示数据，不直接与模型交互。           |
| MVVM | Model-View-ViewModel  | 模型-视图-视图模型 | 进一步发展了MVP，引入了一个视图模型（ViewModel）来处理视图和模型之间的交互，视图通过数据绑定与视图模型通信。           |
| MVU  | Model-View-Update     | 模型-视图-更新     | 一种函数式编程风格的架构模式，强调不可变数据和纯函数，通过一个单一的更新函数来处理所有状态变化。                       |

---

## 1. Model–View–Controller (MVC)

**MVC** 是最早广泛使用的 UI 架构模式，用于将界面、数据和控制逻辑分离。

结构：

```
User → Controller → Model
              ↓
             View
```

职责：

| 组件       | 作用                             |
| ---------- | -------------------------------- |
| Model      | 数据与业务逻辑                   |
| View       | 界面显示                         |
| Controller | 接收用户输入并协调 Model 与 View |

特点：

- Controller 处理用户输入
- Model 更新后通知 View
- View 从 Model 获取数据

典型使用框架：

- ASP.NET Core MVC
- Spring MVC
- Django

适用场景：

- Web 服务端应用
- 传统 GUI 系统

主要问题：

- Controller 容易变得过大（“Fat Controller”）

---

# 2. Model–View–Presenter (MVP)

**MVP** 是在 MVC 基础上的改进模式，进一步加强 **View 与 Model 的解耦**。

结构：

```
User → View → Presenter → Model
               ↓
              View
```

职责：

| 组件      | 作用             |
| --------- | ---------------- |
| Model     | 数据与业务逻辑   |
| View      | UI接口           |
| Presenter | 负责所有 UI 逻辑 |

关键特点：

- View **非常被动（Passive View）**
- Presenter 完全控制 View
- View 通常通过 **接口** 暴露给 Presenter

示例结构：

```
View (interface)
   ↑
Presenter
   ↓
Model
```

应用：

- Android 早期架构
- 游戏 UI

常见框架：

- Unity 项目中常使用 MVP 结构

优点：

- 逻辑集中在 Presenter
- 更容易单元测试

缺点：

- Presenter 可能变得很复杂

---

# 3. Model–View–ViewModel (MVVM)

**MVVM** 是为现代 UI 框架设计的架构模式，核心是 **数据绑定（Data Binding）**。

结构：

```
View  ↔  ViewModel  →  Model
```

职责：

| 组件      | 作用            |
| --------- | --------------- |
| Model     | 数据与业务逻辑  |
| View      | UI              |
| ViewModel | View 的抽象表示 |

关键机制：

**数据绑定**

```
View.Text ↔ ViewModel.Name
```

数据变化：

```
ViewModel 更新
      ↓
PropertyChanged
      ↓
View 自动刷新
```

ViewModel 通常实现：

```
INotifyPropertyChanged
```

典型框架：

- Windows Presentation Foundation
- .NET MAUI
- Vue.js

优点：

- UI 与逻辑高度解耦
- 支持自动 UI 更新
- 非常适合声明式 UI

缺点：

- 依赖数据绑定机制

---

# 4. Model–View–Update (MVU)

**MVU** 是一种 **函数式 UI 架构模式**，由 Elm 提出。

结构：

```
Model → View → Message → Update → Model
```

核心循环：

```
          ┌───────────┐
          │   Model   │
          └─────┬─────┘
                │
                ▼
              View
                │
                ▼
             Message
                │
                ▼
              Update
                │
                ▼
              Model
```

组件：

| 组件   | 作用               |
| ------ | ------------------ |
| Model  | 应用状态           |
| View   | 根据 Model 渲染 UI |
| Update | 根据消息更新 Model |

示例：

```
User 点击按钮
     ↓
Message
     ↓
Update 函数
     ↓
新 Model
     ↓
View 重新渲染
```

应用框架：

- Elm
- Redux
- SwiftUI（部分思想）

优点：

- 状态管理清晰
- 不可变数据
- 易于调试

缺点：

- 对传统 OOP 程序员不直观

---

# 5. 四种架构对比

| 架构 | 控制逻辑    | View 是否主动 | 数据更新方式   | 常见领域  |
| ---- | ----------- | ------------- | -------------- | --------- |
| MVC  | Controller  | 部分主动      | 手动更新       | Web       |
| MVP  | Presenter   | 被动          | Presenter 更新 | 游戏 UI   |
| MVVM | ViewModel   | 自动绑定      | Data Binding   | 现代 UI   |
| MVU  | Update 函数 | 函数式        | 状态驱动       | 函数式 UI |

---

# 6. 设计演化趋势

发展顺序：

```
MVC
  ↓
MVP
  ↓
MVVM
  ↓
MVU
```

变化方向：

```
命令式 UI
      ↓
数据绑定
      ↓
状态驱动 UI
```

---

# 7. 一句话总结

| 架构 | 核心思想            |
| ---- | ------------------- |
| MVC  | Controller 控制界面 |
| MVP  | Presenter 管理 View |
| MVVM | 数据绑定驱动 UI     |
| MVU  | 状态变化驱动 UI     |

---

如果你正在学习 **.NET GUI 开发**，实际常见情况是：

```
WPF / MAUI → MVVM
Unity UI → MVP
Web Server → MVC
函数式 UI → MVU
```
