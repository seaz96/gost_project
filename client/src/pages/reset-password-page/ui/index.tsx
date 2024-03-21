import React, { useContext } from 'react'
import { ResetPasswordForm } from 'widgets/reset-password-form'

import styles from './ResetPasswordPage.module.scss'
import axios from 'axios'
import { UserContext } from 'entities/user'
import { useNavigate } from 'react-router-dom'

const ResetPasswordPage = () => {
  const navigate = useNavigate()
  const {user} = useContext(UserContext)
  const handleSubmit = (oldPassword: string, newPassword: string) => {
    axios.post('localhost:8080/api/accounts/change-password', {
      'login': user?.login,
      'new_password': newPassword,
      'old_password': oldPassword
    }, 
    {
      headers: {
        'Authorization': `Bearer ${localStorage.getItem('jwt_token')}`
      }
    })
    .then(() => navigate('/'))
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