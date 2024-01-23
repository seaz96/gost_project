import React, { useState } from 'react'
import { Button, Input } from 'shared/components';

import styles from './ResetPasswordForm.module.scss'

interface ResetPasswordFormProps {
  handleSubmit: Function
}

const ResetPasswordForm:React.FC<ResetPasswordFormProps> = props => {
  const {
    handleSubmit
  } = props

  const [changePasswordData, setChangePasswordData] = useState({
    oldPassword: '',
    newPassword: '',
    repeatedNewPassword: ''  
  })

  const validateData = (event: React.FormEvent) => {
    event.preventDefault()

    if (changePasswordData.newPassword === changePasswordData.repeatedNewPassword)
      handleSubmit(changePasswordData.oldPassword, changePasswordData.newPassword)
  }

  return (
    <form className={styles.form} onSubmit={(event) => validateData(event)}>
        <Input 
          label='Старый пароль' 
          type='password' 
          value={changePasswordData.oldPassword}
          onChange={(value: string) => setChangePasswordData({...changePasswordData, oldPassword: value})}
        />
        <Input 
          label='Новый пароль' 
          type='password' 
          value={changePasswordData.newPassword}
          onChange={(value: string) => setChangePasswordData({...changePasswordData, newPassword: value})}
        />
        <Input 
          label='Повторите новый пароль' 
          type='password' 
          value={changePasswordData.repeatedNewPassword}
          onChange={(value: string) => setChangePasswordData({...changePasswordData, repeatedNewPassword: value})}
        />
        <Button onClick={() => {}} className={styles.formButton} isFilled type='submit'>Сохранить</Button>
    </form>
  )
}

export default ResetPasswordForm;