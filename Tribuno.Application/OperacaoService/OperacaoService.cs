using Tribuno.Domain;
using Tribuno.Kafka.Contracts;
using Tribuno.Kafka.Producer;
using Tribuno.Repository;
using Tribuno.Application.Constants;

namespace Tribuno.Application.OperacaoService
{
    public class OperacaoService : IOperacaoService
    {
        private readonly IOperacaoRepository operacaoRepository;
        private readonly IKafkaProducer kafkaProducer;


        //KafkaProducer
        public OperacaoService(IOperacaoRepository operacaoRepository, IKafkaProducer kafkaProducer)
        {
            this.operacaoRepository = operacaoRepository;
            this.kafkaProducer = kafkaProducer;
        }

        public async Task<int> Save(Operacao operacao)
        {
            var result = await operacaoRepository.SaveAsync(operacao);

            var evento = new OperacaoCriadaEvent
            {
                EventId = Guid.NewGuid(),
                IdOperacao = result,
                IdUsuario = operacao.IdUsuario,
                NomeOperacao = operacao.NomeOperacao,
                DataEvento = DateTime.UtcNow
            };

            await kafkaProducer.PublishAsync(
                KafkaTopics.OperacaoCriada,
                evento);


            return result;
        }

        public async Task<int> Delete(int id)
        {
            var result = await operacaoRepository.Delete(id);
            return result;
        }

        public async Task<Operacao> Get(int id)
        {
            var result = await operacaoRepository.Get(id);
            return result;
        }

        public async Task<List<Operacao>> GetAll(int idUser)
        {
            var result = await operacaoRepository.GetAll(idUser);
            return result;
        }

        public async Task<int> SaveAsync(Operacao operacao)
        {
            var result = await operacaoRepository.SaveAsync(operacao);

            var evento = new OperacaoCriadaEvent
            {
                EventId = Guid.NewGuid(),
                IdOperacao = result,
                IdUsuario = operacao.IdUsuario,
                NomeOperacao = operacao.NomeOperacao,
                DataEvento = DateTime.UtcNow
            };

            await kafkaProducer.PublishAsync(
                KafkaTopics.OperacaoCriada,
                evento);

            return result;
        }

        public async Task<int> Update(Operacao operacao)
        {
            var result = await operacaoRepository.Update(operacao);
            return result;
        }
    }
}
