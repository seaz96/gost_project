import React, { useContext } from 'react'

import styles from './UsersReview.module.scss'
import { UserContext, userModel } from 'entities/user';
import classNames from 'classnames';
import { Link } from 'react-router-dom';

interface UsersTableProps {
  users: userModel.User[]
}

enum roles {
  'User'='Пользователь',
  'Admin'='Администратор',
  'Heisenberg'='Главный администратор'
}

const UsersReview: React.FC<UsersTableProps> = props => {
  const {user} = useContext(UserContext)
  const {
    users
  } = props

  return (
    <table className={styles.table}>
      <thead>
        <th>№</th>
        <th>Роль</th>
        <th>Логин</th>
        <th>Фио</th>
        <th>Действия</th>
      </thead>
      <tbody>
        {users.map(userData => 
          <tr>
            <td>{userData.id}</td>
            <td>{roles[userData.role]}</td>
            <td>{userData.login}</td>
            <td>{userData.name}</td>
            <td>
              {
                userData.id !== 0 && (user?.role === 'Admin' || user?.role === 'Heisenberg') && 
                <Link to={`/user-edit-page/${userData.id}`} className={classNames(styles.tableButton, 'baseButton', 'filledButton')}>Редактирование</Link>
              }
              
            </td>
          </tr>
        )}
      </tbody>
    </table>
  )
}

export default UsersReview;