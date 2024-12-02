import CryptoJS from 'crypto-js'

const SECRET = import.meta.env.VITE_ENCRYPT_SECRET
const KEY = import.meta.env.VITE_ENCRYPT_KEY

// Ensure key is 32 bytes (256 bits) and IV is 16 bytes (128 bits)
const key = CryptoJS.enc.Utf8.parse(SECRET) // 32 bytes //Move to .env
const iv = CryptoJS.enc.Utf8.parse(KEY) // 16 bytes //Move to .env

export function encrypt(plainText) {
  const encrypted = CryptoJS.AES.encrypt(plainText, key, { iv: iv })
  return encrypted.toString()
}

export function decrypt(cipherText) {
  const bytes = CryptoJS.AES.decrypt(cipherText, key, { iv: iv })
  return bytes.toString(CryptoJS.enc.Utf8)
}
