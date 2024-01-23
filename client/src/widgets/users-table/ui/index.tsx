import React from 'react'

import styles from './UsersReview.module.scss'
import { userModel } from 'entities/user';
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
  const {
    users
  } = props
  console.log(users)
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
        {users.map(user => 
          <tr>
            <td>{user.id}</td>
            <td>{roles[user.role]}</td>
            <td>{user.login}</td>
            <td>{user.name}</td>
            <td>
              <Link to={`/users-edit-page/${user.id}`} className={classNames(styles.tableButton, 'baseButton', 'filledButton')}>Редактирование</Link>
            </td>
          </tr>
        )}
      </tbody>
    </table>
  )
}

export default UsersReview;