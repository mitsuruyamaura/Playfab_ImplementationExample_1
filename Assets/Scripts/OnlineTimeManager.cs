using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace YamaP.Playfab_ImplementationExample_1 {

    public class OnlineTimeManager : MonoBehaviour {
        private const string FORMAT = "yyyy/MM/dd HH:mm:ss";

        private async UniTaskVoid Start() {

            // キャンセレーショントークンの作成
            var ct = this.GetCancellationTokenOnDestroy();

            // OnApplicationQuit メソッド内で非同期処理の待機が出来る状態にする
            await this.GetAsyncApplicationQuitTrigger().OnApplicationQuitAsync(ct);
        }

        /// <summary>
        /// ゲームが終了したときに自動的に呼ばれる
        /// </summary>
        private async UniTask OnApplicationQuit() {  //　async UniTask に変更

            // PlayFab にゲーム終了時の時間を保存
            await UpdateLogOffTimeAsync();

            // 複数の await がある場合には WhenAll で対応

            Debug.Log("ゲームを終了します。");
        }

        /// <summary>
        /// OnApplicationQuit 時に実行するメソッド。今回はログオフ時間をサーバーにセーブ
        /// </summary>
        /// <returns></returns>
        public static async UniTask<bool> UpdateLogOffTimeAsync() {

            string dateTimeString = DateTime.Now.ToString(FORMAT);

            var request = new UpdateUserDataRequest {

                Data = new Dictionary<string, string> { { "LogOffTime", dateTimeString } }
            };

            var response = await PlayFabClientAPI.UpdateUserDataAsync(request);

            if (response.Error != null) {
                Debug.Log("エラー");

                // 要エラーハンドリング

                return false;
            }

            Debug.Log("ログオフ時の時刻セーブ完了");
            return true;
        }
    }
}