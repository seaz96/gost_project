import { useContext } from 'react'
import { ResetPasswordForm } from 'widgets/reset-password-form'
import styles from './ResetPasswordPage.module.scss'
import { UserContext } from 'entities/user'
import { useNavigate } from 'react-router-dom'
import { axiosInstance } from 'shared/configs/axiosConfig'

const ResetPasswordPage = () => {
  const navigate = useNavigate()
  const {user} = useContext(UserContext)
  const handleSubmit = (oldPassword: string, newPassword: string) => {
    axiosInstance.post('/accounts/change-password', {
      'login': user?.login,
      'new_password': newPassword,
      'old_password': oldPassword
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