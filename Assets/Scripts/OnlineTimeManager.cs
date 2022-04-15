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

            // �L�����Z���[�V�����g�[�N���̍쐬
            var ct = this.GetCancellationTokenOnDestroy();

            // OnApplicationQuit ���\�b�h���Ŕ񓯊������̑ҋ@���o�����Ԃɂ���
            await this.GetAsyncApplicationQuitTrigger().OnApplicationQuitAsync(ct);
        }

        /// <summary>
        /// �Q�[�����I�������Ƃ��Ɏ����I�ɌĂ΂��
        /// </summary>
        private async UniTask OnApplicationQuit() {  //�@async UniTask �ɕύX

            // PlayFab �ɃQ�[���I�����̎��Ԃ�ۑ�
            await UpdateLogOffTimeAsync();

            // ������ await ������ꍇ�ɂ� WhenAll �őΉ�

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