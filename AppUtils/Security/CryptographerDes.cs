using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppUtils.Security
{
    public class CryptographerDes : ICryptographer
    {
        private string _key;

        public CryptographerDes(string key)
        {
            if (key.Length < 8)
                throw new ArgumentException("Very small key! (min = 8 symbols)");

            _key = key;
        }

        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return "";

            byte[] strArr = Encoding.Default.GetBytes(plainText);
            byte[] keyArr = Encoding.Default.GetBytes(_key);

            //если длина не кратна 64 битам (8 байтам)
            int diff = strArr.Length % 8;
            if (diff != 0)
            {
                byte[] temp = new byte[strArr.Length + (8 - diff)];
                Array.Copy(strArr, temp, strArr.Length);
                strArr = temp;
            }

            byte[] res = new byte[strArr.Length];
            //шифруем по блокам
            for (int i = 0; i < strArr.Length; i = i + 8)
            {
                byte[] block = new byte[8];
                Array.Copy(strArr, i, block, 0, 8);

                for (int j = 0; j <= 9; j++)
                {
                    //создаем 2 подблока
                    byte[] subblockLeftArr = new byte[4];
                    Array.Copy(block, subblockLeftArr, 4);
                    byte[] subblockRightArr = new byte[4];
                    Array.Copy(block, 4, subblockRightArr, 0, 4);

                    //создаем раундовый ключ
                    byte[] subblockKeyArr = new byte[4];
                    Array.Copy(keyArr, subblockKeyArr, 4);
                    subblockKeyArr = ShiftKeyLeft(keyArr, j);

                    if (j != 9)//если j = 9, то не надо поблоки менять местами
                        block = CryptBlock(subblockLeftArr, subblockRightArr, subblockKeyArr, false);
                    else
                        block = CryptBlock(subblockLeftArr, subblockRightArr, subblockKeyArr, true);
                }
                //скидываем блок в результирующий массив
                Array.Copy(block, 0, res, i, block.Length);
            }
            return Encoding.Default.GetString(res);
        }

        public string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                return "";

            byte[] strArr = Encoding.Default.GetBytes(cipherText);
            byte[] keyArr = Encoding.Default.GetBytes(_key);
            byte[] resArr = new byte[strArr.Length];

            //если длина не кратна 64 битам (8 байтам)
            int diff = strArr.Length % 8;
            if (diff != 0)
                throw new ArgumentException("Incorrect input string!");
            //начинаем с конца
            for (int i = strArr.Length - 8; i >= 0; i = i - 8)
            {
                byte[] block = new byte[8];
                Array.Copy(strArr, i, block, 0, 8);
                //применяем раундовые ключи в обратном порядке
                for (int j = 9; j >= 0; j--)
                {
                    //создаем 2 подблока
                    byte[] subblockLeftArr = new byte[4];
                    Array.Copy(block, subblockLeftArr, 4);
                    byte[] subblockRightArr = new byte[4];
                    Array.Copy(block, 4, subblockRightArr, 0, 4);

                    //создаем раундовый ключ
                    byte[] subblockKeyArr = new byte[4];
                    Array.Copy(keyArr, subblockKeyArr, 4);
                    subblockKeyArr = ShiftKeyLeft(keyArr, j);

                    if (j != 0)//если j = 0, то не надо поблоки менять местами
                        block = DecryptBlock(subblockLeftArr, subblockRightArr, subblockKeyArr, false);
                    else
                        block = DecryptBlock(subblockLeftArr, subblockRightArr, subblockKeyArr, true);
                }
                //скидываем блок в результирующий массив
                Array.Copy(block, 0, resArr, i, block.Length);
            }

            return Encoding.Default.GetString(resArr).Replace('\0', ' ');
        }

        private byte[] CryptBlock(byte[] subblockLeftArr, byte[] subblockRightArr, byte[] subblockKeyArr, bool isLast)
        {
            int subblockLeft = BitConverter.ToInt32(subblockLeftArr, 0);
            int subblockRight = BitConverter.ToInt32(subblockRightArr, 0);
            int subblockKey = BitConverter.ToInt32(subblockKeyArr, 0);

            //xor
            subblockLeft = subblockLeft ^ subblockKey;
            subblockLeftArr = BitConverter.GetBytes(subblockLeft);

            byte[] tmp = new byte[2];
            Array.Copy(subblockLeftArr, tmp, 2);
            Int16 left = BitConverter.ToInt16(tmp, 0);
            Array.Copy(subblockLeftArr, 2, tmp, 0, 2);
            Int16 right = BitConverter.ToInt16(subblockLeftArr, 2);

            //xor
            subblockRight = Modify(left, right) ^ subblockRight;
            subblockRightArr = BitConverter.GetBytes(subblockRight);

            //меняем или не меняем подблоки местами при объединении
            byte[] res = new byte[8];
            if (!isLast)
            {
                Array.Copy(subblockRightArr, res, 4);
                Array.Copy(subblockLeftArr, 0, res, 4, 4);
            }
            else
            {
                Array.Copy(subblockLeftArr, res, 4);
                Array.Copy(subblockRightArr, 0, res, 4, 4);
            }
            return res;
        }

        private byte[] DecryptBlock(byte[] subblockLeftArr, byte[] subblockRightArr, byte[] subblockKeyArr, bool isLast)
        {
            int subblockLeft = BitConverter.ToInt32(subblockLeftArr, 0);
            int subblockRight = BitConverter.ToInt32(subblockRightArr, 0);
            int subblockKey = BitConverter.ToInt32(subblockKeyArr, 0);

            byte[] tmp = new byte[2];
            Array.Copy(subblockLeftArr, tmp, 2);
            Int16 left = BitConverter.ToInt16(tmp, 0);
            Array.Copy(subblockLeftArr, 2, tmp, 0, 2);
            Int16 right = BitConverter.ToInt16(subblockLeftArr, 2);

            //xor
            subblockRight = Modify(left, right) ^ subblockRight;

            //xor
            subblockLeft = subblockLeft ^ subblockKey;
            subblockLeftArr = BitConverter.GetBytes(subblockLeft);
            subblockRightArr = BitConverter.GetBytes(subblockRight);

            //меняем или не меняем подблоки местами при объединении
            byte[] res = new byte[8];
            if (!isLast)
            {
                Array.Copy(subblockRightArr, res, 4);
                Array.Copy(subblockLeftArr, 0, res, 4, 4);
            }
            else
            {
                Array.Copy(subblockLeftArr, res, 4);
                Array.Copy(subblockRightArr, 0, res, 4, 4);
            }
            return res;
        }

        private int Modify(Int16 left, Int16 right)
        {
            //циклический сдвиг влево на 7
            int l = left << 7;
            int r = l >> 16;
            left = (Int16)(l + r);

            //циклический сдвиг вправо на 5
            l = right >> 5;
            r = l << 11;//16-5
            right = (Int16)(l + r);

            //меняем части местами
            int res = (int)left << 16;
            return res + right;
        }

        //возвращает интересующую нас левую половину ключа
        private byte[] ShiftKeyLeft(byte[] keyArr, int i)
        {
            byte[] tmp = new byte[4];
            Array.Copy(keyArr, tmp, 4);
            int left = BitConverter.ToInt32(tmp, 0);
            Array.Copy(keyArr, 4, tmp, 0, 4);
            int right = BitConverter.ToInt32(tmp, 0);

            //циклический сдвиг влево на i * 3
            int l = left << (i * 3);
            int r = right >> (32 - i * 3);
            left = l + r;

            return BitConverter.GetBytes(left);
        }
    }
}
