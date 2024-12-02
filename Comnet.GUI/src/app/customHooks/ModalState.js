/* eslint-disable react-hooks/rules-of-hooks */
import { useState } from 'react'

export default function ModalState() {
  const [modal, setModal] = useState({})
  const openModal = (obj) => {
    setModal(obj)
  }
  const closeModal = () => setModal({})

  return [modal, openModal, closeModal]
}
