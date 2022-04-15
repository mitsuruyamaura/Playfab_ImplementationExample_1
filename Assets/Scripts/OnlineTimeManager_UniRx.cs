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

            // OnApplicationQuit �̎��s�^�C�~���O�Ń��b�Z�[�W�𔭍s���� UniRx �̋@�\
            Observable.OnceApplicationQuit()
                .Subscribe(_ => {
                    QuitGameAsync().Forget();
                });
        }

        /// <summary>
        /// �Q�[�����I�������Ƃ��Ɏ��s
        /// </summary>
        private async UniTask QuitGameAsync() {

            // �P�̏������s���ꍇ
            await UpdateLogOffTimeAsync();

            Debug.Log("�Z�[�u�����@�B");

            // �����̏������s���ꍇ(�K�X�ȏ����ɏ��������Ă�������)
            await UniTask.WhenAll(
                UpdateLogOffTimeAsync(),
                UpdateLogOffTimeAsync(),
                UpdateLogOffTimeAsync(),
                UpdateLogOffTimeAsync()
                );

            Debug.Log("�Z�[�u�����A�B");
            Debug.Log("�Q�[�����I�����܂��B");
        }

        /// <summary>
        /// OnApplicationQuit ���Ɏ��s���郁�\�b�h�B����̓��O�I�t���Ԃ��T�[�o�[�ɃZ�[�u
        /// </summary>
        /// <returns></returns>
        public static async UniTask<bool> UpdateLogOffTimeAsync() {

            string dateTimeString = DateTime.Now.ToString(FORMAT);

            var request = new UpdateUserDataRequest {

                Data = new Dictionary<string, string> { { "LogOffTime", dateTimeString } }
            };

            var response = await PlayFabClientAPI.UpdateUserDataAsync(request);

            if (response.Error != null) {
                Debug.Log("�G���[");

                // �v�G���[�n���h�����O

                return false;
            }

            Debug.Log("���O�I�t���̎����Z�[�u����");
            return true;
        }
    }
}