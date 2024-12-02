import { CloseOutlined } from '@mui/icons-material'
import { Dialog, DialogActions, DialogContent, DialogTitle } from '@mui/material'
import dataConstant from '../../constants/dataConstant'

const DialogCustom = ({
  open,
  title,
  details,
  closeModal,
  popUpButtons,
  className,
  closeIcon,
  ...props
}) => {
  const handleClose = (event, reason) => {
    if (reason === dataConstant.backdropClick) {
      return
    }
    closeModal()
  }

  return (
    <div>
      <Dialog
        open={open}
        onClose={handleClose}
        aria-labelledby="modal-modal-title"
        aria-describedby="modal-modal-description"
        closeAfterTransition
        className={className}
        fullWidth
        {...props}
      >
        <DialogTitle className="px-24 pt-16 pb-8 cm-modal-header">
          {title}
          {closeIcon && <CloseOutlined onClick={closeModal} className="cursor-pointer" />}
        </DialogTitle>
        <DialogContent className={`px-24 pt-8 pb-0 ${popUpButtons && 'pb-20'}`}>
          {details}
        </DialogContent>
        {popUpButtons && <DialogActions>{popUpButtons}</DialogActions>}
      </Dialog>
    </div>
  )
}

export default DialogCustom
