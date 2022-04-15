using Cysharp.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace YamaP.Playfab_ImplementationExample_1 {

    public class OnlineTimeManager_UniRx : MonoBehaviour {
        private const string FORMAT = "yyyy/MM/dd HH:mm:ss";

        private void Start() {

            // OnApplicationQuit の実行タイミングでメッセージを発行する UniRx の機能
            Observable.OnceApplicationQuit()
                .Subscribe(_ => {
                    QuitGameAsync().Forget();
                });
        }

        /// <summary>
        /// ゲームが終了したときに実行
        /// </summary>
        private async UniTask QuitGameAsync() {

            // １つの処理を行う場合
            await UpdateLogOffTimeAsync();

            Debug.Log("セーブ完了①。");

            // 複数の処理を行う場合(適宜な処理に書き換えてください)
            await UniTask.WhenAll(
                UpdateLogOffTimeAsync(),
                UpdateLogOffTimeAsync(),
                UpdateLogOffTimeAsync(),
                UpdateLogOffTimeAsync()
                );

            Debug.Log("セーブ完了②。");
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