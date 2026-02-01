# WordbookApp - iOS移行計画書

## ?? プロジェクト概要

**プロジェクト名**: WordbookApp  
**現在のプラットフォーム**: WPF (.NET 10)  
**ターゲットプラットフォーム**: iOS (および Android、Windows、macOS)  
**移行方法**: .NET MAUI (Multi-platform App UI)  
**作成日**: 2026年2月1日

---

## ?? 移行目標

1. 現在のWPFアプリケーションをiPhoneで動作するように移行
2. クロスプラットフォーム対応（iOS、Android、Windows、macOS）
3. 既存のビジネスロジックとデータ構造の再利用
4. モダンなモバイルUIの実装

---

## ?? 現状分析

### 現在のプロジェクト構成
- **プロジェクトタイプ**: WPF Application
- **フレームワーク**: .NET 10
- **プログラミング言語**: C#
- **UIフレームワーク**: WPF XAML
- **主要ファイル**:
  - `App.xaml` / `App.xaml.cs`
  - `MainWindow.xaml` / `MainWindow.xaml.cs`

### 確認が必要な項目
- [ ] データベース（SQLite、Entity Framework Core等）
- [ ] 外部API連携
- [ ] ファイルストレージ
- [ ] 認証機能
- [ ] プッシュ通知の必要性
- [ ] オフライン機能の要件

---

## ??? アーキテクチャ設計

### 推奨プロジェクト構造

```
WordbookApp/
├── WordbookApp.Core/              # 共有ビジネスロジック
│   ├── Models/                    # データモデル
│   ├── Services/                  # ビジネスサービス
│   ├── Interfaces/                # インターフェース定義
│   └── Helpers/                   # ヘルパークラス
│
├── WordbookApp.Mobile/            # .NET MAUIプロジェクト
│   ├── Platforms/
│   │   ├── iOS/                   # iOS固有のコード
│   │   ├── Android/               # Android固有のコード
│   │   ├── Windows/               # Windows固有のコード
│   │   └── MacCatalyst/           # macOS固有のコード
│   ├── Views/                     # MAUI Pages
│   ├── ViewModels/                # MVVM ViewModels
│   ├── Resources/                 # リソース（画像、フォント等）
│   ├── App.xaml                   # アプリエントリポイント
│   └── MauiProgram.cs             # アプリケーション設定
│
└── WordbookApp.Tests/             # ユニットテスト
```

---

## ?? 移行手順

### フェーズ1: 準備（1-2日）

#### 1.1 開発環境のセットアップ
```bash
# .NET MAUI Workloadのインストール
dotnet workload install maui

# テンプレートの確認
dotnet new maui --help
```

#### 1.2 必要なツールとアカウント
- [x] Visual Studio 2026 (.NET MAUI workload)
- [ ] Xcode (最新版) - Macにインストール
- [ ] Apple Developer Account (年間$99)
- [ ] Mac (物理マシンまたはMac-as-a-Service)
- [ ] Android Studio (Android開発用、オプション)

#### 1.3 プロジェクト分析
- [ ] 既存コードの依存関係を文書化
- [ ] MAUI互換性のないAPIをリストアップ
- [ ] サードパーティライブラリの代替を調査

---

### フェーズ2: プロジェクト作成（2-3日）

#### 2.1 新規MAUIプロジェクトの作成
```bash
cd C:\Projects\WordbookApp
dotnet new maui -n WordbookApp.Mobile
```

#### 2.2 共有ライブラリの作成
```bash
dotnet new classlib -n WordbookApp.Core -f net10.0
```

#### 2.3 ソリューション構成
```bash
dotnet new sln -n WordbookApp
dotnet sln add WordbookApp.Mobile/WordbookApp.Mobile.csproj
dotnet sln add WordbookApp.Core/WordbookApp.Core.csproj
dotnet sln add WordbookApp/WordbookApp.csproj  # 既存WPFプロジェクト（参考用）
```

---

### フェーズ3: コード移行（5-10日）

#### 3.1 モデル層の移行
- [ ] データモデルを `WordbookApp.Core/Models/` に移動
- [ ] Entity定義の移行
- [ ] バリデーションロジックの移行

**変更点**:
- WPF固有の属性を削除
- プラットフォーム非依存のコードに変換

#### 3.2 ビジネスロジック層の移行
- [ ] サービスクラスを `WordbookApp.Core/Services/` に移動
- [ ] データアクセス層の実装
- [ ] 依存性注入の設定

**推奨NuGetパッケージ**:
```xml
<PackageReference Include="Microsoft.Extensions.DependencyInjection" />
<PackageReference Include="CommunityToolkit.Mvvm" Version="8.3.0" />
<PackageReference Include="SQLite-net-pcl" Version="1.9.172" />
```

#### 3.3 ViewModel層の作成
- [ ] `ViewModelBase` の実装
- [ ] 各画面用のViewModelを作成
- [ ] `INotifyPropertyChanged` の実装（またはCommunityToolkit.Mvvm使用）

**例**:
```csharp
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private string title = "単語帳";
    
    [RelayCommand]
    private async Task LoadDataAsync()
    {
        // データ読み込み処理
    }
}
```

#### 3.4 UI層の移行

##### WPF → MAUI XAML変換マッピング

| WPF | .NET MAUI | 備考 |
|-----|-----------|------|
| `Window` | `ContentPage` / `Shell` | Shellが推奨 |
| `TextBlock` | `Label` | |
| `TextBox` | `Entry` / `Editor` | 複数行はEditor |
| `Button` | `Button` | ほぼ同じ |
| `ListBox` | `CollectionView` | より高性能 |
| `DataGrid` | カスタム実装 | サードパーティ製を検討 |
| `Menu` | `Shell.FlyoutItems` | |
| `StackPanel` | `StackLayout` / `VerticalStackLayout` | |
| `Grid` | `Grid` | ほぼ同じ |

##### 画面設計
- [ ] メイン画面（単語一覧）
- [ ] 詳細画面（単語の追加/編集）
- [ ] 設定画面
- [ ] 学習画面（フラッシュカード等）

---

### フェーズ4: プラットフォーム固有実装（3-5日）

#### 4.1 iOS固有の実装
**場所**: `WordbookApp.Mobile/Platforms/iOS/`

- [ ] Info.plist の設定
  - アプリ名、バンドルID
  - 必要なパーミッション（カメラ、マイク等）
  - サポートするiOSバージョン（iOS 14+推奨）

```xml
<key>CFBundleDisplayName</key>
<string>単語帳</string>
<key>CFBundleIdentifier</key>
<string>com.yourcompany.wordbookapp</string>
<key>MinimumOSVersion</key>
<string>14.0</string>
```

- [ ] ライフサイクル処理
- [ ] ネイティブ機能へのアクセス（必要に応じて）

#### 4.2 Android固有の実装（オプション）
**場所**: `WordbookApp.Mobile/Platforms/Android/`

- [ ] AndroidManifest.xml の設定
- [ ] 必要なパーミッション

#### 4.3 Windows固有の実装（オプション）
- [ ] 既存のWPFアプリとの比較・調整

---

### フェーズ5: データ層の実装（2-4日）

#### 5.1 ローカルデータベース（SQLite推奨）

```csharp
// WordbookApp.Core/Services/DatabaseService.cs
using SQLite;

public class DatabaseService
{
    private SQLiteAsyncConnection _database;
    
    public async Task InitializeAsync()
    {
        if (_database != null)
            return;
            
        var dbPath = Path.Combine(
            FileSystem.AppDataDirectory, 
            "wordbook.db3"
        );
        
        _database = new SQLiteAsyncConnection(dbPath);
        await _database.CreateTableAsync<Word>();
    }
    
    public Task<List<Word>> GetWordsAsync()
    {
        return _database.Table<Word>().ToListAsync();
    }
    
    public Task<int> SaveWordAsync(Word word)
    {
        if (word.Id != 0)
            return _database.UpdateAsync(word);
        else
            return _database.InsertAsync(word);
    }
}
```

#### 5.2 依存性注入の設定

```csharp
// MauiProgram.cs
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });
            
        // サービス登録
        builder.Services.AddSingleton<DatabaseService>();
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<MainPage>();
        
        return builder.Build();
    }
}
```

---

### フェーズ6: テストとデバッグ（3-5日）

#### 6.1 ユニットテストの作成
```bash
dotnet new xunit -n WordbookApp.Tests
dotnet sln add WordbookApp.Tests/WordbookApp.Tests.csproj
```

#### 6.2 デバイスでのテスト

**iOSシミュレーター**:
1. Visual Studio → プロジェクトプロパティ → iOS
2. シミュレーターを選択（例: iPhone 15 Pro）
3. F5キーでデバッグ開始

**実機テスト**:
1. Apple Developer Accountでプロビジョニングプロファイル作成
2. デバイスをMacに接続
3. Visual Studioでデバイスをターゲットとして選択

#### 6.3 テスト項目
- [ ] 画面遷移
- [ ] データの保存・読み込み
- [ ] 各種入力操作
- [ ] 回転対応（縦横）
- [ ] ダークモード対応
- [ ] 異なる画面サイズ対応
- [ ] パフォーマンス

---

### フェーズ7: 最適化と仕上げ（2-3日）

#### 7.1 UI/UXの改善
- [ ] アプリアイコンの作成（複数サイズ）
- [ ] スプラッシュスクリーンの設定
- [ ] アニメーション追加
- [ ] ローディングインジケーター
- [ ] エラーハンドリングとユーザーフィードバック

#### 7.2 パフォーマンス最適化
- [ ] 画像の最適化
- [ ] 遅延読み込み（Lazy Loading）
- [ ] 非同期処理の最適化
- [ ] メモリリーク確認

#### 7.3 多言語対応（オプション）
```
// Resources/Strings/AppResources.resx
// Resources/Strings/AppResources.ja.resx
```

---

### フェーズ8: デプロイメント（2-3日）

#### 8.1 App Store Connect準備
1. Apple Developer Portalでアプリ登録
2. App Store Connectでアプリ情報入力
   - アプリ名
   - 説明文
   - スクリーンショット（複数デバイス）
   - キーワード
   - カテゴリ

#### 8.2 リリースビルド作成
```bash
dotnet publish -f net10.0-ios -c Release
```

#### 8.3 App Store審査申請
- [ ] アプリのアーカイブ作成
- [ ] Xcodeから Upload to App Store
- [ ] 審査用情報の入力
- [ ] 審査待ち（通常2-5日）

---

## ??? 必要なNuGetパッケージ

### Core プロジェクト
```xml
<ItemGroup>
  <PackageReference Include="CommunityToolkit.Mvvm" Version="8.3.0" />
  <PackageReference Include="SQLite-net-pcl" Version="1.9.172" />
  <PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
</ItemGroup>
```

### MAUI プロジェクト
```xml
<ItemGroup>
  <PackageReference Include="Microsoft.Maui.Controls" Version="10.0.*" />
  <PackageReference Include="Microsoft.Maui.Controls.Compatibility" Version="10.0.*" />
  <PackageReference Include="CommunityToolkit.Maui" Version="9.0.0" />
  <PackageReference Include="SQLitePCLRaw.bundle_green" Version="2.1.8" />
</ItemGroup>
```

---

## ?? リスクと対策

| リスク | 影響度 | 対策 |
|--------|--------|------|
| Mac環境がない | 高 | Mac-as-a-Service（MacStadium等）の利用 |
| iOS固有のバグ | 中 | 早期段階から実機テストを実施 |
| App Store審査リジェクト | 中 | ガイドライン遵守、事前確認 |
| パフォーマンス問題 | 中 | プロファイリングツール活用 |
| WPF機能の再現困難 | 低 | 代替UI/UXパターンの採用 |

---

## ?? タイムライン概算

| フェーズ | 期間 | 担当 | 状態 |
|----------|------|------|------|
| 1. 準備 | 1-2日 | 開発者 | ? 未開始 |
| 2. プロジェクト作成 | 2-3日 | 開発者 | ? 未開始 |
| 3. コード移行 | 5-10日 | 開発者 | ? 未開始 |
| 4. プラットフォーム固有 | 3-5日 | 開発者 | ? 未開始 |
| 5. データ層実装 | 2-4日 | 開発者 | ? 未開始 |
| 6. テスト | 3-5日 | 開発者/QA | ? 未開始 |
| 7. 最適化 | 2-3日 | 開発者 | ? 未開始 |
| 8. デプロイメント | 2-3日 | 開発者 | ? 未開始 |
| **合計** | **20-35日** | | |

---

## ?? 参考資料

### 公式ドキュメント
- [.NET MAUI ドキュメント](https://learn.microsoft.com/ja-jp/dotnet/maui/)
- [iOS App Distribution Guide](https://developer.apple.com/documentation/xcode/distributing-your-app-for-beta-testing-and-releases)
- [Human Interface Guidelines - iOS](https://developer.apple.com/design/human-interface-guidelines/ios)

### 学習リソース
- [.NET MAUI for Beginners](https://learn.microsoft.com/ja-jp/training/paths/build-apps-with-dotnet-maui/)
- [SQLite with .NET MAUI](https://learn.microsoft.com/ja-jp/dotnet/maui/data-cloud/database-sqlite)

### コミュニティ
- [.NET MAUI GitHub](https://github.com/dotnet/maui)
- [Stack Overflow - .NET MAUI](https://stackoverflow.com/questions/tagged/.net-maui)

---

## ? チェックリスト

### 開始前
- [ ] Mac環境の確保
- [ ] Apple Developer Accountの登録
- [ ] 開発環境のセットアップ完了
- [ ] プロジェクトリポジトリのバックアップ

### 開発中
- [ ] 定期的なコミット（Git）
- [ ] コードレビュー
- [ ] ユニットテストの作成
- [ ] 実機テストの実施

### リリース前
- [ ] 全機能のテスト完了
- [ ] パフォーマンステスト完了
- [ ] セキュリティチェック完了
- [ ] App Storeガイドライン確認
- [ ] スクリーンショット準備完了
- [ ] プライバシーポリシーの作成

---

## ?? 次のアクション

1. **今すぐ開始**: Mac環境とApple Developer Accountの準備
2. **Phase 1実行**: 開発環境セットアップとプロジェクト分析
3. **定期レビュー**: 週1回の進捗確認ミーティング

---

## ?? 質問・サポート

このドキュメントについての質問や追加情報が必要な場合は、プロジェクトチームまでお問い合わせください。

**最終更新日**: 2026年2月1日
