import React, { useContext } from 'react'
import { ResetPasswordForm } from 'widgets/reset-password-form'

import styles from './ResetPasswordPage.module.scss'
import axios from 'axios'
import { UserContext } from 'entities/user'

const ResetPasswordPage = () => {
  const {user} = useContext(UserContext)
  const handleSubmit = (oldPassword: string, newPassword: string) => {
    console.log({
      'login': user?.login,
      'new_password': newPassword,
      'oldPassword': oldPassword
    })
    axios.post('https://backend-seaz96.kexogg.ru/api/accounts/change-password', {
      'login': user?.login,
      'new_password': newPassword,
      'old_password': oldPassword
    }, {headers: {'Authorization': `Bearer ${localStorage.getItem('jwt_token')}`}}).then(responce => console.log(responce))
  }

  return (
    <div>
        <section className={styles.gostSection}>
            <ResetPasswordForm handleSubmit={handleSubmit}/>
        </section>
    </div>
  )
}

export default ResetPasswordPage;