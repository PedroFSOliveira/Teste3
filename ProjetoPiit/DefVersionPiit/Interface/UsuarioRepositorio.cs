using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DefVersionPiit.Models;
using System.Data;
using Npgsql;
using Dapper;
using System.Configuration;

namespace DefVersionPiit.Interface
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private IDbConnection _db = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public Usuario Adicionar(Usuario user)
        {
            var sqlQuery = "INSERT INTO usuario (login, senha, numero_registro, cpf,nome, data_nascimento, email, observacoes, telemovel, telefone,id_endereco) VALUES(@Login, @Senha, @NumeroRegistro, @Cpf, @Nome, @DataNascimento, @Email, @Observacoes, @Telemovel, @Telefone, @IdEndereco); " + "SELECT CAST(SCOPE_IDENTITY() as int)";
            var userId = this._db.Query<int>(sqlQuery, user).Single();
            user.IdUsuario = userId;
            return user;
        }

        public Usuario Atualizar(Usuario user)
        {
            var sqlQuery =
                "UPDATE usuario " +
                "SET nome = @Nome, " +
                "    senha  = @Senha, " +
                "    email     = @Email " +
                "WHERE id_usuario = @IdUsuario";
            this._db.Execute(sqlQuery, user);
            return user;
        }

        public Usuario BuscarInformacoes(int id)
        {
            return this._db.Query<Usuario>("SELECT * FROM usuario WHERE id_usuario = @IdUsuario", new { id }).SingleOrDefault();
            //Aqui pode ser feita uma busca mais específica
        }

        public void Deletar(int id)
        {
            //Falta Terminar de implementar o delete
            var sqlQuery = "DELETE FROM usuario WHERE id_usuario = @id";
            this._db.Execute(sqlQuery, new { id });
        }

        public List<Usuario> Listar()
        {
            return this._db.Query<Usuario>("SELECT id_usuario AS IdUsuario, login, senha, ultimo_login AS UltimoLogin, numero_registro as NumeroRegisto, cpf, nome, data_nascimento AS DataNascimento, email, observacoes, telemovel, telefone, id_endereco AS IdEndereco FROM usuario").ToList();
        }

        public Usuario ProcurarPorId(int id)
        {
            return this._db.Query<Usuario>("SELECT id_usuario AS IdUsuario, login, senha, ultimo_login AS UltimoLogin, numero_registro as NumeroRegisto, cpf, nome, data_nascimento AS DataNascimento, email, observacoes, telemovel, telefone, id_endereco AS IdEndereco FROM usuario WHERE id_usuario = @id", new { id }).SingleOrDefault();
        }
    }
}